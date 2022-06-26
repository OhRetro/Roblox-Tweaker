#Roblox Tweaker
VERSION = ["2.2", "Dev-Unstable"]

#Imports
from os import environ as os_environ, name as os_name
from types import NoneType

try:
    from rich import print
    from oreto_utils import Terminal, File, Folder, Folders, Others

except ImportError as missing_package:
    print(missing_package)
    print("Try running \"pip install -r requirements.txt\"")
    exit(1)

if os_name != "nt":
    print("This program was intended to be run on Windows, Exiting...")
    exit(1)

class RobloxTweaker():
    def __init__(self):
        Terminal.title(f"Roblox Tweaker v{VERSION[0]} {VERSION[1]}")
        Terminal.clear()
        
        self.running = True
        self.exit_code = None
                
        self._roblox_versions_path = os_environ["LocalAppData"]+"/Roblox/Versions"
        self._textures_folders_path = "PlatformContent/pc/textures"
                
        self._exception_texs = ["sky", "brdfLUT.dds", "studs.dds", "wangIndex.dds"]
        
        self.path_file = File("path", ".txt")
        self.path_dir = Folder("", self._roblox_versions_path)
        self.backup_dir = Folder("backup")
        
        if not self.path_file.exists():
            print("[Path File Not Found.]")
            self.writepath()
            Terminal.clear()
            
        if self.path_file.exists:
            self.path = self.path_file.read()
            self.path_folder = Folder("", self.path)
        
        while self.running:
            print(f"Roblox Tweaker v{VERSION[0]} {VERSION[1]}")
            print("What do you want to do?\n")
            print("[1]Delete Textures\n[2]Show Textures List\n[3]Update Roblox Version Path\n[4]Restore Textures from Backup\n[0]Exit\n")
            print(f"Current Roblox Version Path:\n\"{self.path}\"\nType: {self.gettype()}")
            _selected_option = input(">")
            
            if _selected_option in ["D", "d", "1"]:
                self.deletetextures()    
                
            elif _selected_option in ["S", "s", "2"]:
                self.listtextures()
            
            elif _selected_option in ["U", "u", "3"]:
                self.writepath()
                self.path = self.path_file.read()
                self.path_folder = Folder("", self.path)
                
            elif _selected_option in ["R", "r", "4"]:
                self.restoretextures()
                
            elif _selected_option in ["backup"]:
                self.backuptextures()
                
            elif _selected_option in ["E", "e", "0"]:
                Terminal.clear()
                self.running = False
                
            else:
                Terminal.clear()
        
    #Delete Textures
    def deletetextures(self):
        Terminal.clear()
        textures_path = Folder(self._textures_folders_path, self.path)
        textures_list = textures_path.list()
        
        option = None
        while option not in ["a", "all", "l", "leave"]:        
            print("[Delete Textures]\n")
            print("Do you want to delete ALL textures or leave some untouched?\n")
            option = input("[A]ll\n[L]eave (Default) (Recommended)\n\n>") or "l"
        
        if option.lower() in ["l", "leave"]:
            textures_path.deletecontents(self._exception_texs)
            
        elif option.lower() in ["a", "all"]:
            option = None
            while option not in ["y", "yes", "n", "no"]:
                Terminal.clear()
                print("Are you sure you want to DELETE ALL textures?\n")
                option = input("[Y]es\n[N]o (Default)\n\n>") or "n"

            
            if option.lower() in ["y", "yes"]:
                option = None
                while option not in ["y", "yes", "n", "no"]:
                    Terminal.clear()
                    print("Do you want to backup the textures before deleting?\n")
                    option = input("[Y]es (Default) (Recommended)\n[N]o\n\n>") or "y"
                
                if option.lower() in ["y", "yes"]:
                    self.backuptextures()

                Others.countdown(3, "Deleting Textures in")                
                    
                textures_path.deletecontents()
        
        Terminal.clear()
        
        if len(textures_list) <= len(self._exception_texs):
            print("[There's nothing to delete.]\n")
        else: 
            print("[Textures Deleted.]\n")
    
    #List Textures
    def listtextures(self):
        Terminal.clear()
        textures_path = Folder(self._textures_folders_path, self.path)
        try:    
            textures_list = textures_path.list()
            success = True
        except FileNotFoundError:
            print("Unable to detemine type, path is inexistent, Try updating the path.")
            success = False
      
        if success:
            print("[Textures List]\n")
            for texture in textures_list:
                print(texture)
            
        input("\nPress Enter to continue...")
        Terminal.clear()
    
    #Backup Textures
    def backuptextures(self):
        Terminal.clear()
        textures_path = Folder(self._textures_folders_path, self.path)
        if not self.backup_dir.exists():
            self.backup_dir.create()
        print("[Copying contents to the backup directory]")
        if self.backup_dir.list():
            print("[Overwriting backup directory]")
            self.backup_dir.deletecontents()
        textures_path.copycontents(self.backup_dir.folder)
        print("[Done]\n")
    
    #Restore Textures from Backup
    def restoretextures(self):
        Terminal.clear()
        textures_path = Folder(self._textures_folders_path, self.path)
        if self.backup_dir.exists():
            if self.backup_dir.list():
                print("[Restoring Textures]")
                textures_path.deletecontents()
                self.backup_dir.copycontents(textures_path.folder)
                print("[Done]\n")
            else:
                print("[There's nothing to restore.]\n")
        else:
            print("[There's nothing to restore.]\n")
        input("\nPress Enter to continue...")
        Terminal.clear()
        
    #Write File
    def writepath(self):
        while True:
            roblox_version_path = Folders.select("Select a Roblox Version", self._roblox_versions_path, True)
            
            if type(roblox_version_path) != NoneType and roblox_version_path.split("/")[-1].startswith("version-"):
                break
            
        self.path_file.write(roblox_version_path, True)
        Terminal.clear()
        print("[Path File Written.]\n")
        
    #Get which Roblox Type is currently selected
    def gettype(self):
        roblox_exes = ["RobloxPlayerBeta.exe", "RobloxStudioBeta.exe"]
        try:    
            roblox_version_path = Folder("", self.path).list()
        except FileNotFoundError:
            return "Unable to detemine type, path is inexistent, Try updating the path."

        if roblox_exes[0] in roblox_version_path:
            return "Roblox Player"
        elif roblox_exes[1] in roblox_version_path:
            return "Roblox Studio"
        else:
            return "Unknown"
        
if __name__ == "__main__":
    RobloxTweaker()