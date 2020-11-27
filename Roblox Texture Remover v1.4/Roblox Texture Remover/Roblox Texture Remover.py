#Roblox Texture Remover
#Versão:1.4

import os
import os.path
import ctypes
import time
import getpass
import shutil
from tkinter import filedialog
from tkinter import *

ctypes.windll.kernel32.SetConsoleTitleW('Roblox Texture Remover')
username = getpass.getuser()
root = Tk()
root.withdraw()

#-------------------------------------------------------------------------------
#RVF----------------------------------------------------------------------------
print('=======================================================================')
print('Select the Roblox Version Folder.')
RobloxVersionFolder = filedialog.askdirectory(parent=root,
                                  initialdir='C:/Users/' + username + '/AppData/Local/Roblox/Versions',
                                  title='Select the Roblox Version Folder')
print('')
print('Creating Directory file, please wait.')
time.sleep(3)

#-------------------------------------------------------------------------------
#File Generator-----------------------------------------------------------------
FileOutput = 'files/'
FileCreation = os.path.join(FileOutput, 'roblox_version_folder.txt')
File = open(FileCreation, 'w')
FileText = '' + RobloxVersionFolder + ''
File.write(FileText)
File.close()

#-------------------------------------------------------------------------------
#RobloxVersionFolder_dir--------------------------------------------------------
def RobloxVersionFolder_dir():
    with open('files/roblox_version_folder.txt', 'r') as f:
        lines = f.readlines()
        return lines[0].strip()

#dir
RVFdir = RobloxVersionFolder_dir()
TEXdir = '/PlatformContent/pc'
OOFdir = '/content/sounds'

#FULLdir
RobloxTex = RVFdir + TEXdir
RobloxOOF = RVFdir + OOFdir

#Files
TEXfiles = 'files/pc'
OOFfile = 'files/sounds/uuhhh.mp3'

#-------------------------------------------------------------------------------
#Main---------------------------------------------------------------------------
print('Reading files, Please wait.')
time.sleep(1.5)
print('')
time.sleep(5)
print('Starting to remove the textures')
print('And applying the old OOF sound.')
print('')
shutil.rmtree('' + RobloxTex +'')
shutil.copytree(src=TEXfiles, dst=RobloxTex)
time.sleep(1)
print('Textures removed.')
print('')
os.remove('' + RobloxOOF + '/uuhhh.mp3')
shutil.copy(src=OOFfile, dst=RobloxOOF)
time.sleep(1)
print('Old OOF sound applied.')
time.sleep(3)
print('')
print('Done.')
print('The program will close in 5 seconds.')
print('=======================================================================')
time.sleep(5)
