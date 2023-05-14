# Mesh View spot finder

There are 2 branches, the Main branch and the CleanUp branch.
CleanUp branch has more cleaner structure, I've added Helper class there.
However, this has resulted in calculations being a few tenths of a second slower.
The programme is generally fast, taking about 1 second for 10,000 items and about 3 seconds for 20,000 items.
<a name="readme-top"></a>


<!-- GETTING STARTED -->
## Getting Started

This is a Serverless Azure functions project (HTTP trigger function). Using .Net 6, developed in Visual Studio 2022

### Prerequisites

* Visual Studio 2022,
* Insomnia or Postman for Api calls,

### Installation


1. Clone the repo
   ```sh
   git clone https://github.com/KarolRechman/Mesh.git
   ```
2. Open solution in VS 2022
3. Install NuGet packages, it can be done with build
4. Run app
5. Prepere an Http POST request in Insomnia or Postman
* add parameters:
![image](https://github.com/KarolRechman/Mesh/assets/39775135/f419945a-3c1c-41e5-a0c6-458b90b43b48)
![image](https://github.com/KarolRechman/Mesh/assets/39775135/50b23dd3-3b79-4c58-8d01-61c50a180927)
6. Test application by making an HTTP request, to your local host, like this:  http://localhost:7275/api/MeshFunction




<!-- TEST EXAMPLES -->
## TESTS

One of the best times are on these screenshots:

Result with with 10000 elements:
![image](https://github.com/KarolRechman/Mesh/assets/39775135/334f0c42-77fe-4837-9652-038acbe913ed)

Result with with 20000 elements (Record time):
![image](https://github.com/KarolRechman/Mesh/assets/39775135/c4c007e7-a781-404d-9b70-8de053215cc4)

Result with with 20000 elements (CleanUp branch):
![image](https://github.com/KarolRechman/Mesh/assets/39775135/70617717-3963-4f7c-8c78-21204b5684ad)

<p align="right">(<a href="#readme-top">back to top</a>)</p>


