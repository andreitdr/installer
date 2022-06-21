from genericpath import exists
import subprocess
from functions import *
import PySimpleGUI as psg
import os

psg.theme('Dark Amber')


def createCheckBox(layout, typename, interval):
    l = len(layout)
    if l <= interval:
        return [[psg.Checkbox(layout[i], key=f'checkBox_{typename}_{layout[i]}'), ] for i in range(len(layout))]
    else:
        lines = int(l/interval)
        remaining = l - lines*interval
        nlayout = [[]]
        lastline = 0
        for i in range(lines):
            data = []
            for j in range(interval):
                data.append(psg.Checkbox(
                    layout[i*interval+j], key=f'checkBox_{typename}_{layout[i*interval+j]}'))
            nlayout.append(data)
            lastline = i
        data = []
        lastline += 1
        for i in range(remaining):
            data.append(psg.Checkbox(
                layout[lastline*interval + i], key=f'checkBox_{typename}_{layout[lastline*interval+i]}'))

        nlayout.append(data)
        return nlayout


def DownloadBrowsers(browserslist):
    all_browsers = receive(
        'https://sethdiscordbot.000webhostapp.com/Storage/Installers/DownloadLinks/Browsers').split('\n')
    all_browsers = cleanList(all_browsers)
    if not exists('Downloads'):
        os.mkdir('Downloads')
    for i in range(len(all_browsers)):
        for j in range(len(browserslist)):
            if(all_browsers[i].startswith(browserslist[j])):
                url = all_browsers[i].split(',')[1]
                path = f'./Downloads/{browserslist[j]}.exe'
                download(
                    url, path, window['progressBar'], window['label1'])
                subprocess.call(path)
                break


def DownloadApps(applist):
    all_apps = receive(
        'https://sethdiscordbot.000webhostapp.com/Storage/Installers/DownloadLinks/OtherApps').split('\n')
    all_apps = cleanList(all_apps)
    if not exists('Downloads'):
        os.mkdir('Downloads')

    for i in range(len(all_apps)):
        for j in range(len(applist)):
            if(all_apps[i].startswith(applist[j])):
                url = all_apps[i].split(',')[1]
                path = f'./Downloads/{applist[j]}.exe'
                download(
                    url, path, window['progressBar'], window['label1'])
                subprocess.call(path)
                break


def DownloadLaunchers(launcherlist):
    all_launchers = receive(
        'https://sethdiscordbot.000webhostapp.com/Storage/Installers/DownloadLinks/Launchers').split('\n')
    all_launchers = cleanList(all_launchers)
    if not exists('Downloads'):
        os.mkdir('Downloads')

    for i in range(len(all_launchers)):
        for j in range(len(launcherlist)):
            if(all_launchers[i].startswith(launcherlist[j])):
                url = all_launchers[i].split(',')[1]
                path = f'./Downloads/{launcherlist[j]}.exe'
                download(
                    url, path, window['progressBar'], window['label1'])
                subprocess.call(path)
                break


def InstallItems(window: psg.Window):
    items = []
    for i in range(len(browsers)):
        if(window[f'checkBox_browsers_{browsers[i]}'].Get()):
            items.append(browsers[i])
            window[f'checkBox_browsers_{browsers[i]}'].update(
                False, disabled=True)
    DownloadBrowsers(items)

    items.clear()
    for i in range(len(launchers)):
        if(window[f'checkBox_launchers_{launchers[i]}'].Get()):
            items.append(launchers[i])
            window[f'checkBox_launchers_{launchers[i]}'].update(
                False, disabled=True)
    DownloadLaunchers(items)

    items.clear()
    for i in range(len(apps)):
        if(window[f'checkBox_apps_{apps[i]}'].Get()):
            items.append(apps[i])
            window[f'checkBox_apps_{apps[i]}'].update(False, disabled=True)
    DownloadApps(items)

    pass


tabgroup = [
    [psg.Tab('Browsers', createCheckBox(
        browsers, 'browsers', 5), key='Browsers Tab')],
    [psg.Tab('Launchers', createCheckBox(
        launchers, 'launchers', 7), key='Launchers Tab')],
    [psg.Tab('Apps', createCheckBox(apps, 'apps', 4), key='Apps Tab')]
]


windowLayout = [
    [psg.TabGroup(tabgroup, key='Tab Group')],
    [psg.ProgressBar(100, key='progressBar')],
    [psg.Button('Install selected items', key='button1')],
    [psg.Text(pad=[10, 0, 0, 0], key='label1')]
]


window = psg.Window('Install Applications', windowLayout,
                    auto_size_text=True, auto_size_buttons=True, size=(500, 300))

while True:
    event, value = window.read()
    if event == 'button1':
        if AV_MESSAGE_WRITTEN is False:
            win2 = psg.Window('Warning', [[psg.Text('Attention required!')], [psg.Text('Some files may require that you have no antivirus enabled (even Microsoft Defender!)')], [psg.Text('Please disable your antivirus now before proceeding with installation !')],
                                          [psg.Button('Continue', key='button2'), psg.Button('Cancel installation', key='button3', pad=[25, 0, 0, 0])]],
                              auto_size_text=True, auto_size_buttons=True, no_titlebar=True)
            AV_MESSAGE_WRITTEN = True
            while True:
                e1, v1 = win2.read()
                if e1 == 'button2':
                    break
                elif e1 == 'button3':
                    exit()
            win2.close()
        window['button1'].update(disabled=True)
        InstallItems(window)
        window['button1'].update(disabled=False)
        window['progressBar'].update(0)
        window['label1'].update('')
    elif event in (None, 'Exit'):
        break


window.close()
