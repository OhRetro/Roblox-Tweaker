#Roblox Tweaker
_version = "2.0"

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
                
        self.dectect_os()
        
        _platform_content = "/PlatformContent/pc"
        self._textures_folders_path = f"{_platform_content}/textures"
        
        self._textures_folders_list = ["aluminum", "brick", "cobblestone", "concrete", "diamondplate", "fabric", "glass", "granite", "grass",
                                       "ice", "marble", "metal", "pebble", "plastic", "rust", "sand", "slate", "water", "wood", "woodplanks"]

        self.roblox_version_file = File("roblox-version-folder")
        
        if not self.roblox_version_file.exists():
            self.write_rversion_path_file()
        if self.roblox_version_file.exists:
            self._rvf_r = self.roblox_version_file.read()
        
        while True:
            self.menu()

    def menu(self):
        print(f"Roblox Tweaker v{_version} | OS: {self.user_os}\n")
        print("What do you want to do?\n")
        print("[D]elete Textures\n[U]pdate Roblox Version Folder Path\n[E]xit\n")
        if self.user_os == "Linux": print("Update [G]rapejuice Prefix\n")
        print(f"Current Roblox Version Folder Path:\n{self._rvf_r}\n")
        _selected_option = input(">")
        
        if _selected_option in ["D", "d", "1"]:
            self.delete_textures()    
        elif _selected_option in ["U", "u", "2"]:
            self.update_rvf_path()
        elif _selected_option in ["E", "e", "3"]:
            Terminal.clear()
            exit(0)
        elif self.user_os == "Linux" and _selected_option in ["G", "g", 4]:
            self.update_gjp()    
        else:
            Terminal.clear()

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
        _updated = self.write_rversion_path_file()
        Terminal.clear()
        if _updated: print("[Updated]\n")
        else: print("[Operation Canceled]\n")

    def update_gjp(self):
        _updated = self.write_prefix_file()
        Terminal.clear()
        if _updated: print("[Updated]\n")
        else: print("[Operation Canceled]\n")

    def dectect_os(self):
        if os_name == "nt":
            self.user_os = "Windows"
            self._roblox_versions_path = os_environ["LocalAppData"]+"/Roblox/Versions"
            
        elif os_name == "posix":
            self.user_os = "Linux"
            self._grapejuice_prefixes_path = os_environ["HOME"]+"/.local/share/grapejuice/prefixes"
            
            self.grapejuice_prefix_file = File("grapejuice-prefix")
            
            if not self.grapejuice_prefix_file.exists():
                self.write_prefix_file()
            if self.grapejuice_prefix_file.exists:
                self._gjp_r = self.grapejuice_prefix_file.read()

            self._roblox_versions_path = f"{self._gjp_r}/drive_c/Program Files (x86)/Roblox/Versions"

    def select_directory(self, title, initialdir):
        dialog = Tk()
        dialog.withdraw()

        return filedialog.askdirectory(title=title, initialdir=initialdir, parent=dialog)

    def write_rversion_path_file(self):
        self.roblox_version_path = self.select_directory("Select a Roblox Version Folder.", self._roblox_versions_path)
        if type(self.roblox_version_path) == tuple:
            return False

        self.roblox_version_file.write(self.roblox_version_path)
        return True

    def write_prefix_file(self):
        self._grapejuice_prefix_path = self.select_directory("Select a prefix", self._grapejuice_prefixes_path)
        if type(self._grapejuice_prefix_path) == tuple:
            return False

        self.grapejuice_prefix_file.write(self._grapejuice_prefix_path)
        return True


if __name__ == "__main__":
    RobloxTweaker()
