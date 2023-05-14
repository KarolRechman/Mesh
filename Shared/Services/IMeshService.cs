using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Models;

namespace Shared.Services
{
	public interface IMeshService
	{
		Task<List<ViewSpot>?> FindViewSpots(HttpRequest request);
	}
}
