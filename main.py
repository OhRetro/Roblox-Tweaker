#Roblox Tweaker
#Version: 1.0

from tkinter import Tk, filedialog
from time import sleep
from distutils.dir_util import copy_tree
import os
import ctypes
import shutil
import subprocess
import psutil

class Main():
    def __init__(self):
        ctypes.windll.kernel32.SetConsoleTitleW('Roblox Tweaker')
        self.root = Tk()
        self.root.withdraw()
        self.root.iconbitmap('./files/roblox_tweaker_logo.ico')   

        self.roblox_versions_folder_path = os.environ['LocalAppData']+'/Roblox/Versions'
        
        self.roblox_version_folder_path = ''
        self.roblox_version_platform_content_pc_folder_path = ''
        self.roblox_terrain_folder_path = ''
        self.roblox_textures_folder_path = ''
    
        os.system('cls')
        print('Roblox Tweaker 1.0:\n')
        print('[1]Remove textures | [2]Reinstall Roblox files | [3]Exit')
        menu_choice = input('>')
        while menu_choice not in ['1','2','3']:
            os.system('cls')
            print('Roblox Tweaker 1.0:\n')
            print('[1]Remove textures | [2]Reinstall Roblox files | [3]Exit')
            menu_choice = input('>')

        #Delete textures
        if menu_choice == '1':
            self.delete_textures()
        
        #Create Backup
        elif menu_choice == '2':
            self.reinstall_roblox_files()
                    
        #Exit
        elif menu_choice == '3':
            os.system('cls')
            exit(0)
            
        self.__init__()
            
    def delete_textures(self):
        os.system('cls')
        print('Select the version folder to delete the textures from.')
                
        self.get_roblox_version_folder_path()
        if not self.roblox_version_folder_path:
            return        

        if os.path.isdir(self.roblox_version_platform_content_pc_folder_path):
            os.system('cls')
            print('Deleting the folders from "Roblox" folder:\n')
            
            if os.path.isdir(self.roblox_terrain_folder_path):
                print('Deleting "terrain" folder.')
                shutil.rmtree(self.roblox_terrain_folder_path)
                
            if os.path.isdir(self.roblox_textures_folder_path):
                print('Deleting "textures" folder.')
                shutil.rmtree(self.roblox_textures_folder_path)

            print('\nDeleting completed.')
            
        sleep(1.5)

        if not os.path.isdir(self.roblox_terrain_folder_path) or not os.path.isdir(self.roblox_textures_folder_path):
            os.system('cls')
            print('Copying the folders from "files" folder:\n')

            files_platform_content_pc_folder_path = './files/PlatformContent/pc'

            if not os.path.isdir(self.roblox_terrain_folder_path):
                print('Copying "terrain" folder.')
                files_terrain_folder_path = f'{files_platform_content_pc_folder_path}/terrain'
                copy_tree(files_terrain_folder_path, self.roblox_terrain_folder_path)

            if not os.path.isdir(self.roblox_textures_folder_path):
                print('Copying "textures" folder.')
                files_textures_folder_path = f'{files_platform_content_pc_folder_path}/textures'
                copy_tree(files_textures_folder_path, self.roblox_textures_folder_path)
            
            print('\nCopying completed.')
            
        sleep(1.5)
    
    def reinstall_roblox_files(self):
        os.system('cls')
        print('Warning: if you procedure, its going to Reset/Overwrite the folder entirely,')
        print('if you had modified the files before, make sure to backup.\n')
        print('Do you want to continue anyway? Type "yes" to procedure anyway, Type anything else to go back.')
        reinstall_choice = input('>').lower()

        if reinstall_choice != 'yes':
            return

        sleep(0.5)

        os.system('cls')
        print('Select the version folder to Reinstall the files.')

        self.get_roblox_version_folder_path()
        if not self.roblox_version_folder_path:
            return        

        os.system('cls')
        if os.path.isdir(f'{self.roblox_version_folder_path}/ssl'):
            shutil.rmtree(f'{self.roblox_version_folder_path}/ssl')   
            
        sleep(1.5)
        
        subprocess.Popen(f'{self.roblox_version_folder_path}/RobloxPlayerLauncher.exe')

        sleep(0.5)

        while 'RobloxPlayerLauncher.exe' in (i.name() for i in psutil.process_iter()):
            os.system('cls')
            print('Reinstalling in progress.\n')

        print('Reinstalling completed.')

        sleep(1.5)

       
    def get_roblox_version_folder_path(self):      
        self.roblox_version_folder_path = filedialog.askdirectory(title='Select the version folder.',
                                                    initialdir=self.roblox_versions_folder_path,
                                                    parent=self.root)

        self.roblox_version_platform_content_pc_folder_path = f'{self.roblox_version_folder_path}/PlatformContent/pc'
        self.roblox_terrain_folder_path = f'{self.roblox_version_platform_content_pc_folder_path}/terrain'
        self.roblox_textures_folder_path = f'{self.roblox_version_platform_content_pc_folder_path}/textures'
        


if __name__ == '__main__':
    Main()