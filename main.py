#Roblox Tweaker
#Version: 1.2

from tkinter import Tk, filedialog
from time import sleep
from distutils.dir_util import copy_tree
import os
import ctypes
import shutil
import subprocess
import psutil
import requests

_version = "1.2"

class Main():
    
    def __init__(self):
        ctypes.windll.kernel32.SetConsoleTitleW("Roblox Tweaker")
        self.root = Tk()
        self.root.withdraw()
        self.root.iconbitmap("./files/roblox_tweaker_logo.ico")   

        self.roblox_folder_path = os.environ["LocalAppData"]+"/Roblox"
        self.roblox_versions_folder_path = self.roblox_folder_path+"/Versions"
        self.desktop_path = os.environ["UserProfile"]+"/Desktop"
        
        self.roblox_version_folder_path = ""
        self.roblox_version_platform_content_pc_folder_path = ""
        self.roblox_terrain_folder_path = ""
        self.roblox_textures_folder_path = ""
    
        os.system("cls")
        print(f"Roblox Tweaker {_version}:\n")
        print("[1]Delete textures | [2]Reinstall Roblox | [3]Exit")
        menu_choice = input(">")
        while menu_choice not in ["1","2","3"]:
            os.system("cls")
            print(f"Roblox Tweaker {_version}:\n")
            print("[1]Delete textures | [2]Reinstall Roblox | [3]Exit")
            menu_choice = input(">")

        #Delete textures
        if menu_choice == "1":
            self.delete_textures()   
        #Create Backup
        elif menu_choice == "2":
            self.reinstall_roblox()
        #Exit
        elif menu_choice == "3":
            os.system("cls")
            exit(0)
            
        self.__init__()
            
    def delete_textures(self):
        os.system("cls")
        print("Select the version folder to delete the textures from.")
                
        self.get_roblox_version_folder_path()
        if not self.roblox_version_folder_path:
            return        

        os.system("cls")
        print("Do you want to:\n[1]Delete All | [2]Delete 'terrain' | [3]Delete 'textures'")
        delete_choice = input(">")
        while delete_choice not in ["1","2","3"]:
            os.system("cls")
            print("Do you want to:\n[1]Delete All | [2]Delete 'terrain' | [3]Delete 'textures'")
            delete_choice = input(">")
        
        if os.path.isdir(self.roblox_terrain_folder_path) or os.path.isdir(self.roblox_textures_folder_path):
            os.system("cls")
            if delete_choice:
                print("Deleting the folders from 'Roblox' folder:\n")
                
                if os.path.isdir(self.roblox_terrain_folder_path) and delete_choice == "1" or delete_choice == "2":
                    print("Deleting 'terrain' folder.")
                    shutil.rmtree(self.roblox_terrain_folder_path)
                    
                if os.path.isdir(self.roblox_textures_folder_path) and delete_choice == "1" or delete_choice == "3":
                    print("Deleting 'textures' folder.")
                    shutil.rmtree(self.roblox_textures_folder_path)

                print("\nDeleting completed.")
            
        sleep(1.5)

        if not os.path.isdir(self.roblox_terrain_folder_path) or not os.path.isdir(self.roblox_textures_folder_path):
            os.system("cls")
            print("Copying the folders from 'files' folder:\n")

            files_platform_content_pc_folder_path = "./files/PlatformContent/pc"

            if not os.path.isdir(self.roblox_terrain_folder_path) and delete_choice == "1" or delete_choice == "2":
                print("Copying 'terrain' folder.")
                files_terrain_folder_path = f"{files_platform_content_pc_folder_path}/terrain"
                copy_tree(files_terrain_folder_path, self.roblox_terrain_folder_path)

            if not os.path.isdir(self.roblox_textures_folder_path) and delete_choice == "1" or delete_choice == "3":
                print("Copying 'textures' folder.")
                files_textures_folder_path = f"{files_platform_content_pc_folder_path}/textures"
                copy_tree(files_textures_folder_path, self.roblox_textures_folder_path)
            
            print("\nCopying completed.")
            
        sleep(1.5)    
    
    def reinstall_roblox(self):
        os.system("cls")
        print("Warning: if you procedure, its going to Reset/Overwrite the folder entirely,")
        print("if you had modified the files before, make sure to backup.\n")
        print("Do you want to continue anyway? Type 'yes' to procedure anyway, Type anything else to go back.")
        reinstall_choice = input(">").lower()

        if reinstall_choice != "yes":
            return

        sleep(0.5)

        os.system("cls")
        if os.path.isdir(self.roblox_versions_folder_path):
            print("Select the version folder to Reinstall the files.")

            self.get_roblox_version_folder_path()
            if not self.roblox_version_folder_path:
                return        
                
            sleep(1.5)
            
            subprocess.Popen(f"{self.roblox_version_folder_path}/RobloxPlayerLauncher.exe")

            sleep(0.5)

            while "RobloxPlayerLauncher.exe" in (i.name() for i in psutil.process_iter()):
                os.system("cls")
                print("Reinstalling in progress.\n")

            print("Reinstalling completed.")

        elif not os.path.isdir(self.roblox_versions_folder_path):
            self.install_roblox()
            
        sleep(1.5)

    def install_roblox(self):
        print("Looks like you don't have Roblox installed, for some reason... why even bother using the program?")
        print("Do you want to install Roblox? Type 'yes' to procedure anyway, Type anything else to go back.")
        install_choice = input(">").lower()
        
        if install_choice != "yes":
            return

        if not os.path.isdir("./downloaded_files"):
            os.makedirs("./downloaded_files")
        
        sleep(0.5)
        
        os.system("cls")
        request = requests.get("https://www.roblox.com/download/client")
        with open("./downloaded_files/RobloxPlayerLauncher.exe", "wb") as file:
            file.write(request.content)
            
        while not os.path.isfile("./downloaded_files/RobloxPlayerLauncher.exe"):
            os.system("cls")
            print("Downloading in progress.\n")

        print("Downloading completed.")
        
        sleep(1.5)
            
        subprocess.Popen("./downloaded_files/RobloxPlayerLauncher.exe")

        sleep(0.5)

        while "RobloxPlayerLauncher.exe" in (i.name() for i in psutil.process_iter()):
            os.system("cls")
            print("Installing in progress.\n")

        print("Installing completed.")
     
        sleep(1.5)
  
    def get_roblox_version_folder_path(self):      
        self.roblox_version_folder_path = filedialog.askdirectory(title="Select the version folder.",
                                                    initialdir=self.roblox_versions_folder_path,
                                                    parent=self.root)

        self.roblox_version_platform_content_pc_folder_path = f"{self.roblox_version_folder_path}/PlatformContent/pc"
        self.roblox_terrain_folder_path = f"{self.roblox_version_platform_content_pc_folder_path}/terrain"
        self.roblox_textures_folder_path = f"{self.roblox_version_platform_content_pc_folder_path}/textures"
        
if __name__ == "__main__":
    Main()