import time
import ctypes
ctypes.windll.kernel32.SetConsoleTitleW('python')
print('python')
py = input()
if py == 'python':
    print('python')
    time.sleep(2)
else:
    print('not python')
    time.sleep(2)
