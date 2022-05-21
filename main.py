#Roblox Tweaker
_version = ["2.2", "Dev-Unstable"]

#Imports
try:
    from oreto_utils import Terminal, File, Folder, Folders, Others

except ImportError as missing_package:
    print(missing_package)
    exit(1)

finally:
    from os import environ as os_environ, name as os_name
    from types import NoneType
    from time import sleep as t_sleep

if os_name != "nt":
    print("This program was intended to be run on Windows, Exiting...")
    exit(1)

class RobloxTweaker():
    def __init__(self):
        Terminal.title(f"Roblox Tweaker v{_version[0]} {_version[1]}")
        Terminal.clear()
        
        self.running = True
        self.exit_code = None
                
        self._roblox_versions_path = os_environ["LocalAppData"]+"/Roblox/Versions"
        self._textures_folders_path = "PlatformContent/pc/textures"
                
        self._exception_folders = ["sky", "brdfLUT.dds", "studs.dds", "wangIndex.dds"]
        
        self.path_file = File("path", ".txt")
        self.path_dir = Folder("", self._roblox_versions_path)
        self.backup_dir = Folder("backup")

        if not self.path_dir.exists():
            self.path_dir.create()
        
        if not self.path_file.exists():
            print("[Path File Not Found.]")
            self.writepath()
            Terminal.clear()
            
        if self.path_file.exists:
            self.path = self.path_file.read()
            self.path_folder = Folder("", self.path)
        
        while self.running:
            print(f"Roblox Tweaker v{_version[0]} {_version[1]}")
            print("What do you want to do?\n")
            print("[D]elete Textures\n[S]how Textures List\n[U]pdate Roblox Version Path\n[R]estore Textures from Backup\n[E]xit\n")
            print(f"Current Roblox Version Path:\n\"{self.path}\"\nType: {self.gettype()}\n{self.outdated()}")
            _selected_option = input(">")
            
            if _selected_option in ["D", "d", "1"]:
                self.deletetextures()    
                
            elif _selected_option in ["S", "s", "2"]:
                self.listtextures()
            
            elif _selected_option in ["U", "u", "3"]:
                self.writepath()
                self.path = self.path_file.read()
                self.path_folder = Folder("", self.path)
                
            elif _selected_option in ["E", "e", "4"]:
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
            textures_path.deletecontents(self._exception_folders)
            
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
                
                #TODO: Backup
                if option.lower() in ["y", "yes"]:
                    Terminal.clear()
                    before_backup = len(textures_path.list())
                    #textures_path.copycontents(self.backup_dir.folder)
                    print("[Copying contents to the backup directory]\n")
                    while len(self.backup_dir.list()) == before_backup:
                        print(f"{len(self.backup_dir.list())}/{before_backup}")
                        t_sleep(.5)
                        Terminal.clearlines(1)

                    print("[Done]\n")
                
                counter = 3
                while counter != 0:
                    print(f"Deleting contents in [{counter}]")
                    t_sleep(1)
                    Terminal.clearlines(1)
                    
                #textures_path.deletecontents()
        
        Terminal.clear()
        
        if len(textures_list) <= len(self._exception_folders):
            print("[There's nothing to delete.]\n")
        else: 
            print("[Textures Deleted.]\n")
    
    #List Textures
    def listtextures(self):
        Terminal.clear()
        textures_path = Folder(self._textures_folders_path, self.path)
        textures_list = textures_path.list()
        print("[Textures List]\n")
        
        for texture in textures_list:
            print(texture)
            
        input("\nPress Enter to continue...")
        Terminal.clear()
    
    #Write File
    def writepath(self):
        while True:
            roblox_version_path = Folders.select("Select a Roblox Version", self._roblox_versions_path, True)
            
            if type(roblox_version_path) != NoneType and roblox_version_path.split("/")[-1].startswith("version-"):
                break
            
        self.path_file.write(roblox_version_path)
        Terminal.clear()
        print("[Path File Written.]\n")
        
    #Get which Roblox Type is currently selected
    def gettype(self):
        roblox_exes = ["RobloxPlayerBeta.exe", "RobloxStudioBeta.exe"]
        roblox_version_path = Folder("", self.path).list()

        if roblox_exes[0] in roblox_version_path:
            return "Roblox Player"
        elif roblox_exes[1] in roblox_version_path:
            return "Roblox Studio"
        else:
            return "Unknown"

    #Check if Roblox Version Path is empty to declare as outdated or not
    def outdated(self):
        path_list = self.path_folder.list()
        return "[Outdated]\n" if len(path_list) <= 1 else ""
        
if __name__ == "__main__":
    RobloxTweaker()