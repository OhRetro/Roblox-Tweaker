#Roblox Tweaker
_version = "2.0" #WIP

#Imports
import os

if os.name == "nt":
    import ctypes
    ctypes.windll.kernel32.SetConsoleTitleW(f"Roblox Tweaker {_version}")

def clear_console():
    if os.name == "nt": os.system("cls")
    else: os.system("clear")

class Main:
    def __init__(self):
        print("\nReworking on it...\n")

if __name__ == "__main__":
    Main()
