#region License
// Author: Nate Kohari <nkohari@gmail.com>
// Copyright (c) 2007-2009, Enkari, Ltd.
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//   http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion
#region Using Directives
using System;
using Ninject.Activation.Caching;
#endregion

namespace Ninject.Activation.Hooks
{
	/// <summary>
	/// A hook that resolves a context.
	/// </summary>
	public class ContextResolutionHook : IHook
	{
		/// <summary>
		/// Gets the context that will be resolved by the hook.
		/// </summary>
		public IContext Context { get; private set; }

		/// <summary>
		/// Gets or sets the cache.
		/// </summary>
		public ICache Cache { get; private set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="ContextResolutionHook"/> class.
		/// </summary>
		/// <param name="context">The context to resolve.</param>
		/// <param name="cache">The cache to use to look up instances for re-use.</param>
		public ContextResolutionHook(IContext context, ICache cache)
		{
			Context = context;
			Cache = cache;
		}

		/// <summary>
		/// Resolves the instance associated with this hook.
		/// </summary>
		/// <returns>The resolved instance.</returns>
		public object Resolve()
		{
			lock (Context.Binding)
			{
				Context.Instance = Cache.TryGet(Context);

				if (Context.Instance != null)
					return Context.Instance;

				Context.Instance = Context.GetProvider().Create(Context);
				Cache.Remember(Context);

				return Context.Instance;
			}
		}
	}
}