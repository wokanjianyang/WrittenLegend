﻿#region copyright
// ---------------------------------------------------------------
//  Copyright (C) Dmitriy Yukhanov - focus [https://codestage.net]
// ---------------------------------------------------------------
#endregion

#if UNITY_2018_1_OR_NEWER

namespace CodeStage.AntiCheat.Genuine.CodeHash
{
	internal abstract class BaseWorker
	{
		public HashGeneratorResult Result { get; private set; }
		public bool IsBusy { get; private set; }

		public virtual void Execute()
		{
			IsBusy = true;
		}

		protected virtual void Complete(HashGeneratorResult result)
		{
			Result = result;
			IsBusy = false;
		}
	}
}

#endif