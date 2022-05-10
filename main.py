#Roblox Tweaker
_version = ["2.2", "Stable"]

#Imports
try:
    from oreto_utils import Terminal, File, Folder, Folders

except ImportError as missing_package:
    print(missing_package)
    exit(1)

finally:
    from os import environ as os_environ, name as os_name

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
        self._textures_folders_path = "/PlatformContent/pc/textures"
                
        self._exception_folders = ["sky", "brdfLUT.dds", "studs.dds", "wangIndex.dds"]
        
        self.path_file = File("path", ".txt")
        self.path_dir = Folder("", self._roblox_versions_path)
        
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
            print("[D]elete Textures\n[U]pdate Roblox Version Folder Path\n[E]xit\n")
            print(f"Current Roblox Version Folder Path:\n\"{self.path}\"\nType: {self.gettype()}\nOutdated Path: {self.outdated()}\n")
            _selected_option = input(">")
            
            if _selected_option in ["D", "d", "1"]:
                self.deletetextures()    
                
            elif _selected_option in ["U", "u", "2"]:
                self.writepath()
                self.path = self.path_file.read()
                self.path_folder = Folder("", self.path)
                
            elif _selected_option in ["E", "e", "3"]:
                Terminal.clear()
                self.exit_code = 0
                self.running = False
                
            else:
                Terminal.clear()
        
    #Delete Textures
    def deletetextures(self):
        textures_path = Folder(self._textures_folders_path, self.path)
        textures_list = textures_path.list()
        textures_path.deletecontents(self._exception_folders)
                
        Terminal.clear()
        
        if len(textures_list) == len(self._exception_folders):
            print("[There's nothing to delete.]\n")
        else: 
            print("[Textures Deleted.]\n")
        
    #Write File
    def writepath(self):
        while True:
            roblox_version_path = Folders.select("Select Roblox Version Folder", self._roblox_versions_path, True)
                
            if roblox_version_path.split("/")[-1].startswith("version-"):
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
        return path_list == []
        
if __name__ == "__main__":
    rt = RobloxTweaker()
    exit(rt.exit_code)