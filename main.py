#Roblox Tweaker
_version = "2.0"  # WIP

#Imports
from oreto_utils import Terminal
from oreto_utils import File
from os import environ as os_environ
from os import name as os_name
from tkinter import Tk, filedialog
from shutil import rmtree as sh_rmtree

class RobloxTweaker():
    def __init__(self):
        Terminal.title(f"Roblox Tweaker v{_version}")
        Terminal.clear()
        
        self.dialog = Tk()
        self.dialog.withdraw()
        
        self.dectect_os()
        
        _platform_content = "/PlatformContent/pc"
        self._textures_folders_path = f"{_platform_content}/textures"
        
        self._textures_folders_list = ["aluminum", "brick", "cobblestone", "concrete", "diamondplate", "fabric", "glass", "granite", "grass",
                                       "ice", "marble", "metal", "pebble", "plastic", "rust", "sand", "slate", "water", "wood", "woodplanks"]

        self.roblox_version_file = File("roblox-version-folder")
        
        if not self.roblox_version_file.exists():
            self.write_path_file()
        if self.roblox_version_file.exists:
            self._rvf_r = self.roblox_version_file.read()
        
        while True:
            self.menu()

    def menu(self):
        print(f"Roblox Tweaker v{_version} | OS: {self.user_os}\n")
        print("What do you want to do?\n")
        print("[D]elete Textures\n[U]pdate Roblox Version Folder Path\n[E]xit\n")
        print(f"Current Roblox Version Folder Path:\n{self._rvf_r}\n")
        _selected_option = input(">")
        
        if _selected_option in ["D", "d", "1"]:
            self.delete_textures()    
        elif _selected_option in ["U", "u", "2"]:
            self.update_rvf_path()
        elif _selected_option in ["E", "e", "3"]:
            exit(0)
        

    def delete_textures(self):
        if not self.roblox_version_file.exists():
            Terminal.clear()
            print("[There is no Version File.]\n")
            return
        
        try:
            for _texture_folder in self._textures_folders_list:
                sh_rmtree(f"{self._rvf_r}{self._textures_folders_path}/{_texture_folder}")

            Terminal.clear()
            print("[Done.]\n")

        except FileNotFoundError:
            Terminal.clear()
            print("[There is no textures available to delete.]\n")

    def update_rvf_path(self):
        _updated = self.write_path_file()
        Terminal.clear()
        if _updated: print("[Updated]\n")
        else: print("[Operation Canceled]\n")

    def dectect_os(self):
        if os_name == "nt":
            self._roblox_versions_path = os_environ["LocalAppData"]+"/Roblox/Versions"
            self.user_os = "Windows"
            self.select_directory_message = "Select the Version Folder of Roblox."
            
        elif os_name == "posix":
            self._roblox_versions_path = os_environ["HOME"]+"/.local/share/grapejuice/prefixes"
            self.user_os = "Linux"
            self.select_directory_message = "Search within the roblox prefix you are using for the roblox version folder."

    def select_directory(self):
        return filedialog.askdirectory(title=self.select_directory_message, initialdir=self._roblox_versions_path, parent=self.dialog)

    def write_path_file(self):
        self.roblox_version_path = self.select_directory()
        if type(self.roblox_version_path) == tuple:
            return False

        self.roblox_version_file.write(self.roblox_version_path)
        return True

if __name__ == "__main__":
    RobloxTweaker()
