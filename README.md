# TaxiEmptyLoadedRateCalculator
A tool calculates empty-loaded rate according to several taxi running snapshots 
###运行说明：
* 首先保证本地安装了excel并且excel已经激活
* 为vs添加github插件来导入本项目，执行工具->拓展和更新，搜索github，然后安装。参考链接：http://blog.csdn.net/u012391923/article/details/51879404?ref=myrecommend
* 如果读取Excel文件报错，那么执行工具->NuGet包管理器->程序包管理控制台，输入Install-Package Microsoft.Office.Interop.Excel
* 如果Config.cs报错，在项目上右键->添加->引用，搜索System.Configuration，然后添加。
