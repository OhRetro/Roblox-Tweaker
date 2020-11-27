#Roblox Texture Remover
#Versão:1.0

import os
import os.path
import ctypes
import time
import getpass
import shutil

ctypes.windll.kernel32.SetConsoleTitleW('Roblox Texture Remover')
username = getpass.getuser()

#-------------------------------------------------------------------------------
#RVFdir-------------------------------------------------------------------------
def read_file():
    with open('files/roblox_version_folder.txt', 'r') as f:
        lines = f.readlines()
        return lines[0].strip()

RVFdir = read_file()
TEXdir = '/PlatformContent/pc/textures'
FULLdir = RVFdir + TEXdir
FILESdir = 'files/textures'
#-------------------------------------------------------------------------------
#Main---------------------------------------------------------------------------
print('=======================================================================')
print("NOTE: Open [Config RVF] if you haven't.")
print('Please wait 5 seconds')
print('=======================================================================')
time.sleep(5)
print('Starting to remove the texture')
shutil.rmtree('' + FULLdir +'')
time.sleep(3)
shutil.copytree(src=FILESdir, dst=FULLdir)
print('')
print('Done.')
print('=======================================================================')
time.sleep(4)
