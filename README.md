# DoNetPE
破坏.net dll正常的PE结构，使得一些反编译工具例如dnspy无法正常反编译，可用于保护unity3d mono下的dll，不会影响游戏正常的运行。与其它方案一起加密时，请务必先使用此工具，此工具依赖于解析正常的.net 正常的pe文件。
##使用方法
- 将你的.net dll文件直接拖进编译好的donetpe.exe上，生成已后缀名_encrypt.dll即是被破坏的pe结构的dll
