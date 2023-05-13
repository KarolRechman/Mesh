namespace Shared.Services;

class MeshService : IMeshService
{
	var formdata = await req.ReadFormAsync();
	var file = formdata.Files["mesh"];
	int numberOfSpots = int.Parse(req.Form["numberOfSpots"]);
}