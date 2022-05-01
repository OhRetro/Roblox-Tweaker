#Roblox Tweaker
_version = ["2.1", "Dev"]

#Imports
try:
    from oreto_utils import Terminal
    from oreto_utils import File

except ImportError as missing_package:
    print(missing_package)
    exit(1)

finally:
    from os import environ as os_environ, name as os_name
    from tkinter import Tk, filedialog
    from shutil import rmtree as sh_rmtree
    from contextlib import suppress as ctxl_suppress

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
        
        self._textures_folders_list = ["aluminum", "brick", "cobblestone", "concrete", "diamondplate", "fabric", "glass", "granite", "grass",
                                       "ice", "marble", "metal", "pebble", "plastic", "rust", "sand", "slate", "water", "wood", "woodplanks"]

        self.roblox_version_file = File(file_name="roblox-version-folder", file_ext="")
        
        if not self.roblox_version_file.exists():
            self.write_file()
            
        if self.roblox_version_file.exists:
            self._rvf_r = self.roblox_version_file.read()
        
        while self.running:
            self.menu()

    def menu(self):
        print(f"Roblox Tweaker v{_version[0]} {_version[1]}")
        print("What do you want to do?\n")
        print("[D]elete Textures\n[U]pdate Roblox Version Folder Path\n[E]xit\n")
        print(f"Current Roblox Version Folder Path:\n\"{self._rvf_r}\"\nType: {self.get_type()}\n")
        _selected_option = input(">")
        
        if _selected_option in ["D", "d", "1"]:
            self.delete_textures()    
            
        elif _selected_option in ["U", "u", "2"]:
            self.update_path()
            
        elif _selected_option in ["E", "e", "3"]:
            Terminal.clear()
            self.exit_code = 0
            self.running = False
            
        else:
            Terminal.clear()

    #Delete Textures
    def delete_textures(self):
        for _texture_folder in self._textures_folders_list:
            with ctxl_suppress(FileNotFoundError):
                sh_rmtree(f"{self._rvf_r}/{self._textures_folders_path}/{_texture_folder}")
                
        Terminal.clear()
        print("[Textures Deleted.]\n")
        
    #Update Roblox Version Folder Path
    def update_path(self):
        _updated = self.write_file()
        self._rvf_r = self.roblox_version_file.read()
        Terminal.clear()
        
        if _updated:
            print("[Updated]\n")
        else:
            print("[Operation Canceled]\n")

    #Select Directory
    def select_directory(self, title, initialdir):
        dialog = Tk()
        dialog.withdraw()

        return filedialog.askdirectory(title=title, initialdir=initialdir, parent=dialog)

    #Write File
    def write_file(self):
        self.roblox_version_path = self.select_directory("Select a Roblox Version Folder.", self._roblox_versions_path)
        
        if self.roblox_version_path == "":
            return False

        self.roblox_version_file.write(self.roblox_version_path)
        return True

    #Get which Roblox Type is currently selected
    def get_type(self):
        rop = File(file_name="RobloxPlayerBeta", file_ext=".exe", file_path=f"{self._rvf_r}")
        ros = File(file_name="RobloxStudioBeta", file_ext=".exe", file_path=f"{self._rvf_r}")

        if rop.exists():
            return "Roblox Player"
        elif ros.exists():
            return "Roblox Studio"

if __name__ == "__main__":
    _rt = RobloxTweaker()
    exit(_rt.exit_code)