# Axibug.MHFSaveDataAutoConverter

### 这是一个开源的 Monster Hunter Frontier 各版本之间的玩家存档，自动化迁移工具。

### This is an open-source tool for automated migrating player savedata between different versions of Monster Hunter Frontier.

本程序目的在于解决自动化解决MHF任意版本到MHF任何版本的相互转换问题。采用各个版本分别配置，但是通用转换的方式，方便添加各种版本的数据读取配置和中间兼容互转。

The purpose of this program is to automatically solve the problem of converting between any version of MHF to any other version of MHF. Each version is configured separately, but using a universal conversion method facilitates the addition of data reading configurations for various versions and ensures compatibility and inter-version conversion.

长远核心目标是为了支持一个完整的继承链 :MH2Dos =>MHF Season=>MHF FW5 => MHFG => MHFZ

The long-term core objective is to support a complete inheritance chain: MH2Dos =>MHF Season=>MHF FW5 => MHFG => MHFZ

适用于任何基于原版存档的服务端或采用原版数据通过cmp20110113\0压缩的MHF服务端

(如 Erupe 等服务器程序)。

It is applicable to any server based on the original version archive or MHF server using original data compressed through cmp20110113\0 

(such as Erupe and other server programs).

本程序采用“原始版本存档-->通用存档模型-->目标版本”的方式开发，今后用于各种各样的版本相互转换，你甚至可以给savefile降低MHF版本且安全，目前接近完美支持了MHF-FW5到MHF-GG的迁移。

本程序，已经在我的中国的服务器使用（axibug.com）：皓月云MHF-FW5和皓月MHFG两个服务器之间，作为网站调用的类库，自动化的让玩家自助在版本之间迁移自己的savedata，通过我的两个版本的网站.

This program is developed using the approach of "original version archiving --> universal archiving model --> target version", which will be used for various version conversions in the future. You can even downgrade the MHF version for savefiles safely, and it currently provides near-perfect support for migrating from MHF-FW5 to MHF-GG.

This program has been used on my chinese server(axibug.com): between the two servers, Haoyue MHF-FW5 and Haoyue MHFG, as a library called by the website, it automatically allows players to migrate their savedata between versions through my two versions of the website

#### 使用方法 / HOW TO USE

首先配置数据库密码和两个版本的数据库名称，在cfg.ini中,然后运行程序。

First, configure the database password and the names of the two versions of the database in cfg.ini, and then run the program.

然后根据控制台提示操作即可，完全自动化。

Then, simply follow the prompts on the console, and it will be fully automated.

如下，是控制台方式使用流程：

Below is the process for using the console:

Step1.选择来源版本 / Select the original MHF version

	读取配置
	配置读取成功
	==欢迎使用 皓月云MHF存档迁移工具 ver 0.1.0 Axibug.MHFSaveAutoConverter==
	Step1.选择要继承的角色的原始MHF版本: Select the original MHF version of the character you want to inherit:
	[0]FW5
	[1]GG

示例输入，如FW5

	> 0

Step2.选择目标版本 / Select the target MHF version:

	Step2.请选择目标MHF版本: Please select the target MHF version:
	[0]FW5
	[1]GG

	>1

Step3.选择需要继承存档的角色ID / Enter the character ID

	step3.请输入[FW5]版本中的源Characters表中的角色ID: Please enter the character ID from the source Characters table in [FW5] version:

	>1248

	===>操作的角色ID:1248
	[FW5]角色1248:[PlayerName]数据加载完毕

Step4. 升级到目标版本或者仅Dump / Upgrade To Target Version Or Dump Only

	step4.是否升级存档到GG(y),或仅Dump来自的FW5存档(n)？
	step4.Do you want to upgrade the save file to GG(y), or just dump the FW5 save file(n)?

	>y

	FW5_1248_角色数据PlayerName,已保存数据到:src_FW5_cid_1248_yyyyMMddHHmmss_decrypt.bin

此时数据会将原始MHF版本的数据解压后，dump保存到程序同级目录的：./src_FW5_cid_1248_yyyyMMddHHmmss_decrypt.bin

若选择了升级版本执行：

	.... 这里会详细输出存档内容 / original savedata information

	====读取模板数据==== 
	====尝试开始写入====
	
	..... 这里会输出升级后的详细内容 / new savedata information

	====写入完毕====

	GG_1248_角色数据PlayerName,已保存数据到:update_GG_cid_1248_yyyyMMddHHmmss_fixed.bin

此时数据会将已经升级到MHF版本的数据解压后，dump保存到程序同级目录的：./update_GG_cid_1248_yyyyMMddHHmmss_fixed.bin

Step 5.[可选]选择是否将新角色写入新数据库的用户下 / [Optional] inserted into the Characters table in the target version database, associated with your userid

	step 5.[可选]将升级数据导入到目标数据库。若回车则取消。输入目标userid,则在目标版本数据库下Characters表插入数据据，并关联您的userid
	Step 5. [Optional] Import the upgrade data into the target database. Press Enter to cancel. Enter the target userid, and the data will be inserted into the Characters table in the target version database, associated with your userid

	>1

	写入目标GG数据库成功!

接下来就愉快的游戏吧。

Let's Play Game. Have Fun !! :D