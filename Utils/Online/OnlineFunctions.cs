﻿internal static class OnlineFunctions
{
    /// <summary>
    /// Downloads a <see cref="Stream"/> and saves it to another <see cref="Stream"/>.
    /// </summary>
    /// <param name="client">The <see cref="HttpClient"/> that is used to download the file</param>
    /// <param name="url">The url to the file</param>
    /// <param name="destination">The <see cref="Stream"/> to save the downloaded data</param>
    /// <param name="progress">The <see cref="IProgress{T}"/> that is used to track the download progress</param>
    /// <param name="cancellation">The cancellation token</param>
    /// <returns></returns>
    internal static async Task DownloadFileAsync(this HttpClient client, string url, Stream destination,
                                                 IProgress<float>? progress = null, IProgress<long>? downloadedBytes = null, CancellationToken cancellation = default)
    {
        using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
        {
            var contentLength = response.Content.Headers.ContentLength;

            using (var download = await response.Content.ReadAsStreamAsync())
            {
                // Ignore progress reporting when no progress reporter was 
                // passed or when the content length is unknown
                if (progress == null || !contentLength.HasValue)
                {
                    await download.CopyToAsync(destination, 81920);
                    return;
                }

                // Convert absolute progress (bytes downloaded) into relative progress (0% - 100%)
                var relativeProgress = new Progress<long>(totalBytes =>
                    {
                        progress.Report((float)totalBytes / contentLength.Value * 100);
                        downloadedBytes?.Report(totalBytes);
                    }
                );
                // Use extension method to report progress while downloading
                await download.CopyToOtherStreamAsync(destination, 8192, relativeProgress, cancellation);
                progress.Report(1);
            }
        }
    }

    public static async Task CopyToOtherStreamAsync(this Stream stream, Stream destination, int bufferSize, IProgress<long>? progress = null, CancellationToken cancellationToken = default)
    {
        if (stream == null) throw new ArgumentNullException(nameof(stream));
        if (destination == null) throw new ArgumentNullException(nameof(destination));
        if (bufferSize <= 0) throw new ArgumentOutOfRangeException(nameof(bufferSize));
        if (!stream.CanRead) throw new InvalidOperationException("The stream is not readable.");
        if (!destination.CanWrite) throw new ArgumentException("Destination stream is not writable", nameof(destination));

        byte[] buffer = new byte[bufferSize];
        long totalBytesRead = 0;
        int bytesRead;
        while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken).ConfigureAwait(false)) != 0)
        {
            await destination.WriteAsync(buffer, 0, bytesRead, cancellationToken).ConfigureAwait(false);
            totalBytesRead += bytesRead;
            progress?.Report(totalBytesRead);
        }
    }

    /// <summary>
    /// Read contents of a file as string from specified URL
    /// </summary>
    /// <param name="url">The URL to read from</param>
    /// <param name="cancellation">The cancellation token</param>
    /// <returns></returns>
    internal static async Task<string> DownloadStringAsync(string url, CancellationToken cancellation = default)
    {
        using (var client = new HttpClient())
        {
            return await client.GetStringAsync(url);
        }
    }
}