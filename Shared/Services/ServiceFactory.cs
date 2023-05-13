using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Services
{
	public static class ServiceFactory
	{
		private static IMeshService _meshService = null;

		public static IMeshService GetMeshService()
		{
			if (_meshService != null)
			{
				return _meshService;
			}

			_meshService = new MeshService();

			return _meshService;
		}

	}
}
