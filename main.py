from genericpath import exists
import zipfile
from clint.textui import progress
import requests
import os


print('Please select one option:')
print('[1] Download C# made installer - more stable (100 MB +)')
print('[2] Download default installer - can be unstable (10MB)')

URL = ''

try:
    userInput = int(input('Selected option = '))
    if userInput not in (1, 2):
        print('Your choice is invalid. The installer will stop')
        exit()
    elif userInput == 1:
        URL = 'https://uccb47f13273107a7d8c4d6b3019.dl.dropboxusercontent.com/cd/0/get/BnlzrW0alAAmb3_H7MkjCV-LXyBZBRFBcsEiKrchXkc7nzjuUo6sFOvxw8Bo6jENK3v5LajsRb0s4WwqkJdnDBqNmDG8RD6T8GA6gMXchp6sGFKJAT9qDuAL6FFf71ahtiPUq9kQZoh3NT5giJPi36LWfWfolzSjYkjsSJLFrEt_UN66c_DoVz2KViwfRQpddEs/file#'
    else:
        URL = 'https://uc3107f3cd39f85f2545fc0e7bf3.dl.dropboxusercontent.com/cd/0/get/Bnn3Kl7XO0mvqPYVUN6uZPpn8ZCeSoUDREWxNSiLzUBUez4h0fgQQEwdDfawpgMWtrem2Z1ZKmcFLO9xXEKX4xCMfjoV7GvpZStvUuqwhbzUXMPha4_JrrCx-4LD_KgC5tDmFaND9OVfGCCEihZOX19ufizTA5ySixZAxQGeFOOURwDZnr8mtGZHwKxgScFCvoM/file#'
except Exception as e:
    print(e)
    exit()
print('Downloading installer...')
print('Please wait...')

r = requests.get(URL, stream=True)
path = 'Installer.zip'
with open(path, 'wb') as f:
    total_length = int(r.headers.get('content-length'))
    for chunk in progress.bar(r.iter_content(chunk_size=1024), expected_size=(total_length/1024) + 1):
        if chunk:
            f.write(chunk)
            f.flush()

print('Download complete!')
print('Now extracting...')

os.remove('./Installer.zip')

with zipfile.ZipFile(path, 'r') as zip_ref:
    entries = zip_ref.filelist.__len__()
    print(f'Extracting {entries} files to ./')
    for entry in zip_ref.filelist:
        zip_ref.extract(entry)
        print(
            f'Extracted [{entry.orig_filename} / {entry.filename}]. Size: {entry.file_size}')

print('\nExtracted.\n')

os.chdir('./win-x64')
if exists('Installer.exe'):
    os.system('Installer.exe')

else:
    print('Error: Installer.exe not found.')
    print('Please try again.')
exit()
