import os
import os.path
import ctypes
import time
import getpass
from tkinter import filedialog
from tkinter import *

ctypes.windll.kernel32.SetConsoleTitleW('RVF')
username = getpass.getuser()
root = Tk()
root.withdraw()


#-------------------------------------------------------------------------------
#RVF----------------------------------------------------------------------------
print('=======================================================================')
print('Select the Version Folder.')
RVF = filedialog.askdirectory(parent=root,
                                  initialdir='C:/Users/' + username + '/AppData/Local/Roblox/Versions',
                                  title='Select the Version Folder')
print('')
print('Done, now open [Roblox Texture Remover]')
print('=======================================================================')
time.sleep(4)

#-------------------------------------------------------------------------------
#File Generator-----------------------------------------------------------------
FileOutput = 'files/'
FileCreation = os.path.join(FileOutput, 'roblox_version_folder.txt')
File = open(FileCreation, 'w')
FileText = '' + RVF + ''
File.write(FileText)
File.close()
