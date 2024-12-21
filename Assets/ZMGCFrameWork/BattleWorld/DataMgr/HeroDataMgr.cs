/*--------------------------------------------------------------------------------------
* Title: 数据脚本自动生成工具
* Author: 铸梦xy
* Date:2024/12/15 15:05:36
* Description:数据层,主要负责游戏数据的存储、更新和获取
* Modify:
* 注意:以下文件为自动生成，强制再次生成将会覆盖
----------------------------------------------------------------------------------------*/
using System.Collections.Generic;

namespace ZMGC.Battle
{

	/// <summary>
	/// 存储英雄相关的数据
	/// </summary>
	public  class HeroDataMgr : IDataBehaviour
	{
	
		private Dictionary<int, int[]> mHeroNormalSkillCfgDic = new Dictionary<int, int[]>() 
		{
			{1000,new int[] {1001,1002,1003} }, //鬼剑士

		};

		private Dictionary<int, int[]> mHeroSkillCfgDic = new Dictionary<int, int[]>()
		{
			{1000, new int[] {1004,1005,1007,1008} } ,//鬼剑士
		};


		public int[] GetHeroNormalSkillArr(int heroid)
		{
			return mHeroNormalSkillCfgDic[heroid];
		}


		public int[] GetHeroSkillArr(int heroid)
		{
			return mHeroSkillCfgDic[heroid];
		}


		 public  void OnCreate()
		 {
		
		 }
		
		 public  void OnDestroy()
		 {
		
		 }
	
	}
}
