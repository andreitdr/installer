from typing import List
import requests
import PySimpleGUI as pg


def cleanList(list: List[str]):
    for i in range(len(list)):
        list[i] = list[i].rstrip()
    return list


def receive(url) -> str:
    r = requests.get(url)
    return r.text


def convertBytes(bytes):
    if bytes < 1024:
        return bytes, 'B'
    elif bytes < 1024*1024:
        return bytes/1024, 'KB'
    elif bytes < 1024*1024*1024:
        return bytes/1024/1024, 'MB'
    elif bytes < 1024*1024*1024*1024:
        return bytes/1024/1024/1024, 'GB'
    else:
        return bytes/1024/1024/1024/1024, 'TB'


def download(url, path, pbar: pg.ProgressBar, label: pg.Text):
    r = requests.get(url, stream=True)
    label.update(
        f'Downloading {str(path).split("/")[len(str(path).split("/"))-1]}')
    with open(path, 'wb') as f:
        header = r.headers.get('content-length')
        if header is None:
            f.write(r.content)
            f.flush()
            pbar.update(100)
            return True
        total_length = int(header)
        downloaded = 0
        tbytes = convertBytes(total_length)
        for chunk in r.iter_content(chunk_size=4096):
            if chunk:
                downloaded += len(chunk)
                pbar.update(downloaded*100/total_length)
                f.write(chunk)
                cbytes = convertBytes(downloaded)

                label.update(
                    f'Downloading {str(path).split("/")[len(str(path).split("/"))-1]} [{cbytes[0]:.2f} {cbytes[1]} / {str(round(tbytes[0],2))} {tbytes[1]}]')
        f.flush()
        f.close()
    return True


try:
    browsers = receive(
        'https://sethdiscordbot.000webhostapp.com/Storage/Installers/BrowserList').split('\n')
    launchers = receive(
        'https://sethdiscordbot.000webhostapp.com/Storage/Installers/LauncherList').split('\n')
    apps = receive(
        'https://sethdiscordbot.000webhostapp.com/Storage/Installers/OtherAppsList').split('\n')
except:
    print('Failed to fetch data ... Please try again later')
    exit()
cleanList(browsers)
cleanList(apps)
cleanList(launchers)

browsers.sort()
launchers.sort()
apps.sort()


AV_MESSAGE_WRITTEN = False
