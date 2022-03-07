<div style="margin-bottom: 1%; padding-bottom: 2%;">
	<img align="right" width="100px" src="https://dx29.ai/assets/img/logo-Dx29.png">
</div>

Dx29 Bioentity
==============================================================================================================================================

[![Build Status](https://f29.visualstudio.com/Dx29%20v2/_apis/build/status/DEV-MICROSERVICES/Dx29.BioEntity?branchName=develop)](https://f29.visualstudio.com/Dx29%20v2/_build/latest?definitionId=72&branchName=develop)
[![MIT license](https://img.shields.io/badge/license-MIT-brightgreen.svg)](http://opensource.org/licenses/MIT)

### **Overview**

This project is used to obtain symptom and disease information.

It is used in the [Dx29 application](https://dx29.ai/) and therefore how to integrate it is described in the [Dx29 architecture guide](https://dx29-v2.readthedocs.io/en/latest/index.html).

It is programmed in C#, and the structure of the project is as follows:

>- src folder: This is made up of multimple folders which contains the source code of the project.
>>- Dx29.BioEntity.Web.API. In this project is the implementation of the controllers that expose the API methods.
>>- Dx29.BioEntity. It is this project that contains the logic to perform the relevant operations.
>>- Dx29. Used as library to add the common or more general functionalities used in Dx29 projects programmed in C#.
>- .gitignore file
>- README.md file
>- manifests folder: with the YAML configuration files for deploy in Azure Container Registry and Azure Kubernetes Service.
>- pipeline sample YAML file. For automatizate the tasks of build and deploy on Azure.


<p>&nbsp;</p>

### **Getting Started**

####  1. Configuration: Pre-requisites

This project uses:
>- The Human Phenotype Ontology. Find out more at [http://www.human-phenotype-ontology.org](http://www.human-phenotype-ontology.org)
>- MONDO. Find out more at [https://github.com/monarch-initiative/mondo](https://github.com/monarch-initiative/mondo)
>- ORPHANET. Find out more at: [http://www.orphadata.org/cgi-bin/index.php](http://www.orphadata.org/cgi-bin/index.php)
>- OMIM. Find out more at: [https://www.omim.org/](https://www.omim.org/)

Please note the updates of these files for this project. 

This project doesn’t need any secret value.

<p>&nbsp;</p>

####  2. Download and installation

DDownload the repository code with `git clone` or use download button.

We use [Visual Studio 2019](https://docs.microsoft.com/en-GB/visualstudio/ide/quickstart-aspnet-core?view=vs-2022) for working with this project.

<p>&nbsp;</p>

####  3. Latest releases

The latest release of the project deployed in the [Dx29 application](https://dx29.ai/) is: v0.15.01.

<p>&nbsp;</p>

#### 4. API references

**Conditions or diseases** 
>- Describe: To obtain information on diseases from a list of IDs and in a given language.
>>- GET request
>>- URL: ```http://localhost/api/v1/Conditions/describe?id=<List<string>>&lang=<lang>```
>>- Result: Dictionary with key equal to the identifier of the request and value the list of results obtained. This list of results will be composed of objects with the items:
>>>- Disease Id (string)
>>>- Disease Name (string)
>>>- Disease Description (string)
>>>- Disease comment (string)
>>>- Alternates: List of strings of the alternates of this disease
>>>- Synonyms: list of synonyms with this information: Label, Scope,Type and Xrefs (all strings)
>>>- Categories, Parents, Children and Consider: List of related items with Id and Name.
>>>- PubMeds and XRefs (List strings)
>>>- IsObsolete (bool) and ReplacedBy (Reference of the new disease with its Id and Name).
>>>- ToString method for printing the disease as: "Id: Name".
**Phenotype** 
>- Describe: To obtain information on phenotypes from a list of IDs and in a given language.
>>- GET request
>>- GET URL: ```http://localhost/api/v1/Phenotypes/describe?id=<List<string>>&lang=<lang>```
>>- POST request
>>- POST URL: ```http://localhost/api/v1/Phenotypes/describe?lang=<lang>```
>>- Body request: List symptom identifiers (strings).
>>- Result: Dictionary with key equal to the identifier of the request and value the list of results obtained. This list of results will be composed of objects with the items:
>>>- Symptom Id (string)
>>>- Symptom Name (string)
>>>- Symptom Description (string)
>>>- Symptom comment (string)
>>>- Alternates: List of strings of the alternates of this symptom
>>>- Synonyms: list of synonyms with this information: Label, Scope,Type and Xrefs (all strings)
>>>- Categories, Parents, Children and Consider: List of related items with Id and Name.
>>>- PubMeds and XRefs (List strings)
>>>- IsObsolete (bool) and ReplacedBy (Reference of the new Symptom with its Id and Name).
>>>- ToString method for printing the symptom as: "Id: Name".
>- Predecessors: Get the predeccesors of a symptom with configurable tree depth (-1 is equal to ALL predecessors of a symptom in the tree).
>>- Both GET and POST request are available
>>- GET URL: ```http://localhost/api/v1/Phenotypes/predecessors?<List<string>>&depth=<int_depth>```
>>- POST URL: ```http://localhost/api/v1/Phenotypes/predecessors?depth=<int_depth>```
>>- Body request: List symptom identifiers (strings).
>>- Result request: Dictionary with key equal to the identifier of the request and value is an object with the predecessors information.
**Terms** 
>- Describe: To obtain information on diseases or symptoms regardless of type, from a list of IDs and in a given language.
>>- Both GET and POST request are available.
>>>- URL GET: ```http://localhost/api/v1/Terms/describe?id=<List<string>>&lang=<lang>```
>>>- URL POST: ```http://localhost/api/v1/Terms/describe?lang=<lang>```
>>>- POST body request: List of ids (strings).

<p>&nbsp;</p>

### **Build and Test**

#### 1. Build

We could use Docker. 

Docker builds images automatically by reading the instructions from a Dockerfile – a text file that contains all commands, in order, needed to build a given image.

>- A Dockerfile adheres to a specific format and set of instructions.
>- A Docker image consists of read-only layers each of which represents a Dockerfile instruction. The layers are stacked and each one is a delta of the changes from the previous layer.

Consult the following links to work with Docker:

>- [Docker Documentation](https://docs.docker.com/reference/)
>- [Docker get-started guide](https://docs.docker.com/get-started/overview/)
>- [Docker Desktop](https://www.docker.com/products/docker-desktop)

The first step is to run docker image build. We pass in . as the only argument to specify that it should build using the current directory. This command looks for a Dockerfile in the current directory and attempts to build a docker image as described in the Dockerfile. 
```docker image build . ```

[Here](https://docs.docker.com/engine/reference/commandline/docker/) you can consult the Docker commands guide.

<p>&nbsp;</p>

#### 2. Deployment

To work locally, it is only necessary to install the project and build it using Visual Studio 2019. 

The deployment of this project in an environment is described in [Dx29 architecture guide](https://dx29-v2.readthedocs.io/en/latest/index.html), in the deployment section. In particular, it describes the steps to execute to work with this project as a microservice (Docker image) available in a kubernetes cluster:

1. Create an Azure container Registry (ACR). A container registry allows you to store and manage container images across all types of Azure deployments. You deploy Docker images from a registry. Firstly, we need access to a registry that is accessible to the Azure Kubernetes Service (AKS) cluster we are creating. For this purpose, we will create an Azure Container Registry (ACR), where we will push images for deployment.
2. Create an Azure Kubernetes cluster (AKS) and configure it for using the prevouos ACR
3. Import image into Azure Container Registry
4. Publish the application with the YAML files that defines the deployment and the service configurations. 

This project includes, in the Deployments folder, YAML examples to perform the deployment tasks as a microservice in an AKS. 
Note that this service is configured as "ClusterIP" since it is not exposed externally in the [Dx29 application](https://dx29.ai/), but is internal for the application to use. If it is required to be visible there are two options:
>- The first, as realised in the Dx29 project an API is exposed that communicates to third parties with the microservice functionality.
>- The second option is to directly expose this microservice as a LoadBalancer and configure a public IP address and DNS.

>>- **Interesting link**: [Deploy a Docker container app to Azure Kubernetes Service](https://docs.microsoft.com/en-GB/azure/devops/pipelines/apps/cd/deploy-aks?view=azure-devops&tabs=java)


<p>&nbsp;</p>

### **Contribute**

Please refer to each project's style and contribution guidelines for submitting patches and additions. The project uses [gitflow workflow](https://nvie.com/posts/a-successful-git-branching-model/). 
According to this it has implemented a branch-based system to work with three different environments. Thus, there are two permanent branches in the project:
>- The develop branch to work on the development environment.
>- The master branch to work on the test and production environments.

In general, we follow the "fork-and-pull" Git workflow.

>1. Fork the repo on GitHub
>2. Clone the project to your own machine
>3. Commit changes to your own branch
>4. Push your work back up to your fork
>5. Submit a Pull request so that we can review your changes

The project is licenced under the **(TODO: LICENCE & LINK & Brief explanation)**

<p>&nbsp;</p>
<p>&nbsp;</p>

<div style="border-top: 1px solid !important;
	padding-top: 1% !important;
    padding-right: 1% !important;
    padding-bottom: 0.1% !important;">
	<div align="right">
		<img width="150px" src="https://dx29.ai/assets/img/logo-foundation-twentynine-footer.png">
	</div>
	<div align="right" style="padding-top: 0.5% !important">
		<p align="right">	
			Copyright © 2020
			<a style="color:#009DA0" href="https://www.foundation29.org/" target="_blank"> Foundation29</a>
		</p>
	</div>
<div>

	
