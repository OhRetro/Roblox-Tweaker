#Roblox Tweaker
_version_info = [[2,1,0], "Dev"]
_version = f"{_version_info[0][0]}.{_version_info[0][1]}.{_version_info[0][2]}, {_version_info[1]}"

#Imports
try:
    from oreto_utils import Terminal
    from oreto_utils import File

except ImportError as missing_package:
    print(missing_package)
    exit(0)

finally:
    from os import environ as os_environ
    from tkinter import Tk, filedialog
    from shutil import rmtree as sh_rmtree

class RobloxTweaker():
    def __init__(self):
        Terminal.title(f"Roblox Tweaker v{_version}")
        Terminal.clear()
                
        self._roblox_versions_path = os_environ["LocalAppData"]+"/Roblox/Versions"
        _platform_content = "/PlatformContent/pc"
        self._textures_folders_path = f"{_platform_content}/textures"
        
        self._textures_folders_list = ["aluminum", "brick", "cobblestone", "concrete", "diamondplate", "fabric", "glass", "granite", "grass",
                                       "ice", "marble", "metal", "pebble", "plastic", "rust", "sand", "slate", "water", "wood", "woodplanks"]

        self.roblox_version_file = File("roblox-version-folder", "")
        
        if not self.roblox_version_file.exists():
            self.write_rversion_path_file()
        if self.roblox_version_file.exists:
            self._rvf_r = self.roblox_version_file.read()
        
        while True:
            self.menu()

    def menu(self):
        print(f"Roblox Tweaker v{_version}")
        print("What do you want to do?\n")
        print("[D]elete Textures\n[U]pdate Roblox Version Folder Path\n[E]xit\n")
        print(f"Current Roblox Version Folder Path:\n{self._rvf_r}\n")
        _selected_option = input(">")
        
        if _selected_option in ["D", "d", "1"]:
            self.delete_textures()    
        elif _selected_option in ["U", "u", "2"]:
            self.update_rvf_path()
        elif _selected_option in ["E", "e", "3"]:
            Terminal.clear()
            exit(0)
        else:
            Terminal.clear()

    #Delete Textures
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

    #Update Roblox Version Folder Path
    def update_rvf_path(self):
        _updated = self.write_rversion_path_file()
        Terminal.clear()
        if _updated: print("[Updated]\n")
        else: print("[Operation Canceled]\n")

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


if __name__ == "__main__":
    RobloxTweaker()
