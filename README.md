# azuredevpos-intro-workshop

## Preparando lo necesario
Algunos flujos que se van a definir pierden un poco el sentido si todos los pasos los ejecuta el mismo usuario. Aunque se puede hacer y no hay problema, resulta más ilustrativo si se hace con más de un usuario. Si tienes más de una cuenta de correo, puedes usarlas para simular con ellas diferentes usuarios. Otra opción es hacer esto en pareja con alguien y ser cada uno contribuidor del otro.
### Cuenta de Azure DevOps donde vamos a trabajar
+ Tener una cuenta de outlook (de las normales, valen las gratuitas) [Puedes crear una aquí](https://outlook.live.com/owa/) o una cuenta de Github
+ [Crear una organización en Azure DevOps con la cuenta de outlook](https://azure.microsoft.com/en-us/services/devops/)
    + Selecciona Start free si tienes cuenta de Outlook
    + Selecciona Start fee with GitHub si tienes cuenta de GitHub: deberás autorizar a que Microsoft tenga acceso a tu cuenta y completar el proceso de verificación de que efectivamente eres tú

Una vez completado el proceso de crear la organización en Azure DevOps, os remitirá a una url del tipo https://dev.azure.com/nombreOrganizacion . Si olvidáis el nombre de la organización, siempre podéis volver a la url donde creasteis la organización (https://azure.microsoft.com/en-us/services/devops/) y desde ahí seleccionar "Sign in to Azure DevOps" para que os lleve a vuestra organización
#### Instalación de la Azure Cli

+ [Instala la Azure Cli](https://docs.microsoft.com/en-us/cli/azure/install-azure-cli?view=azure-cli-latest)
+ Añade la extensión de Azure DevOps:

       az extension add --name azure-devops
    
    + Si al ejecutar el comando en Windows no reconoce 'az' como una expresión válida, es que falta añadir al path C:\Program Files\Microsoft SDKs\Azure\.NET SDK\v2.9
    + En Linux puede que tengais que ejecutar el comando con `sudo`
+ [Crea un Token de Acceso personal (PAT)](https://docs.microsoft.com/en-us/azure/devops/organizations/accounts/use-personal-access-tokens-to-authenticate?view=azure-devops&tabs=preview-page#create-a-pat): el scope a elegir será de "Full access". Copia y guarda el token cuando se muestre ya que será el que usemos para hacer el login desde la Azure Cli
+ Configura como default la organización creada anteriormente para no tener que escribirla cada vez que ejecutemos un comando:

       az devops configure --defaults organization=https://dev.azure.com/tu-organizacion
### Código y Framework
En la práctica vamos a trabajar con un proyecto desarrollado en [.NET 5](https://dotnet.microsoft.com/download) y los pantallazos que me veréis del IDE serán de [Visual Studio Code](https://code.visualstudio.com/)

Evidentemente, no es obligatorio ni usar mi proyecto de ejemplo, ni trabajar con ese lenguaje, ni con ese IDE. Mi elección se basa tanto en que a mí me resultan familiares y cómodos como en que son multiplataforma.

#### Instalación de .Net Core
+ Windows: Se descarga el instalable de [.NET 5](https://dotnet.microsoft.com/download)
+ Linux: En la mayoría de las distribuciones se puede [instalar a través del package manager](https://docs.microsoft.com/en-us/dotnet/core/install/linux). Otra opción es descargarse los binarios de [aquí](https://dotnet.microsoft.com/download/dotnet/5.0) y realizar la instalación manualmente

#### Instalación de Visual Studio Code
Desde su [página](https://code.visualstudio.com/download) se puede descargar el instalador para Windows y los paquetes para Linux

## Creando nuestro primer proyecto y añadiendo a colaboradores
### Desde la interfaz
#### Crear un nuevo proyecto
Desde la página de nuestra nueva organización de Azure DevOps nos saldrá por defecto una página para crear un nuevo proyecto:
![Página de nuestra nueva organización donde se nos pide el nombre de un nuevo proyecto a crear y si queremos que su visibilidad sea pública o privada](Imagenes/Nuevo_Proyecto.PNG)

Lo nombramos como "Code Motion Demo Project", seleccionamos como visibilidad Private y damos al botón de crear un nuevo proyecto.

Una vez creado, si volvemos a la página principal de la organización podemos ver listado nuestro nuevo proyecto creado y nos aparece el botón de crear un nuevo proyecto desde la interfaz de igual manera que el que acabamos de crear.
#### Añadir un colaborador
Hay dos maneras de hacerlo:
+ Desde el proyecto, se añade un colaborador (con el correo electrónico) a uno de los grupos creados
    + Desde Project settings > Permissions > Se selecciona un grupo
    ![Imagen de los settings de permisos de un proyecto. En la esquina inferior izquierda, Project Settings aparece remarcado y con un 1. A la derecha del menú principal vertical hay otro menú secundario vertical, sobre ese menú aparece Permissions remarcado y un un número 2 al lado. A la derecha, aparece otro área con el título de Permissions que muestra un listado de los grupos de la organización. El grupo de Contributors aparece remarcado con un 3 al lado](Imagenes/Acceso_grupos_repositorio.PNG)
    + En el apartado de Members del grupo, se selecciona Add > Se escribe el email y se guarda
    + Por defecto se creará con un nivel de acceso de Stakeholder, por lo que no podrá ver los repositorios. El nivel de acceso se cambia desde los settings de la organización
+ Desde la organización: en la esquina inferior derecha aparece el enlace a Organization settings
    + En el menú de Organization Settings, se selecciona Users
    + Pasamos el ratón sobre el colaborador añadido previamente. Nos aparecerán tres puntitos a la derecha del registro. Si hacemos click, sobre el menú que aparece, seleccionamos la opción de Change access level y cambiamos de Stakeholder a Basic y guardamos
    + En el menú de Users, seleccionamos Add users. Añadimos el email de otro colaborador, seleccionamos el Access level como Basic, los proyectos a los que queremos añadirles y en qué grupo lo vamos a incluir (seleccionamos Project Contributors) y guardamos.
    + Si accediésemos al grupo de Contributors del proyecto ahora veríamos al usuario que acabamos de añadir

### Por línea de comando

Lo primero es logarse:

       az devops login

El token que solicita es el Token de acceso personal que obtuvimos antes.
En caso de que tuviésemos una subscripción a Azure, también sería válido el login con 

       az login

En todos los comandos a continuación no vamos a especificar la organización porque ya configuramos cuál era la que se cogía por defecto. Si no lo hubiésemos hecho, tendríamos que indicarla con el parámetro --org a cada uno de los comandos

#### Crear un nuevo proyecto
Creamos un proyecto con visibilidad privada y nombre "CodeMotion WorkShop Project Command Line"

       az devops project create --name "CodeMotion WokShop Project Command Line"

Por defecto nos lo crea con visibilidad privada.
Como vamos a estar usando este proyecto para todos los comandos por consola, vamos a definirlo como el proyecto por defecto para no tener que definir el parámetro de proyecto en todos los comandos

       az devops configure --defaults project="CodeMotion WokShop Project Command Line"

#### Añadir un colaborador
Añadimos a un usuario a la organización con una licencia de tipo Basic (y no le enviamos un email al hacerlo)

       az devops user add --email-id email --license-type express --send-email-invite false

Una vez añadido el usuario a la organización, lo añadimos al grupo de Contributors del proyecto. Para añadir al usuario al proyecto necesitamos saber el "descriptor" del grupo. Para obtenerlo, listamos los grupos del proyecto y del output obenido recogemos el descriptor del grupo de Contributors

       az devops security group list

Buscamos el grupo con el displayName de Contributors:

           {
      "description": "Members of this group can add, modify, and delete items within the team project.",
      "descriptor": "vssgp.Uy0xLTktQRU1SWM3NDI0NS0xODAyMTAwNzY2LTI1MzU3MjIwNTItMjMyNjkzMzU3OS0zMjY3NDI5NDYzLTEtNzYxNDIxODkyLTI3NDg1NDQ4NDUtMjk3NzI0MjIzOS0zMTUwMzI1NTI0",
      "displayName": "Contributors",
      "domain": "vstfs:///Classification/TeamProject/ebdf696b-2497-440c-8ab2-304bc2c10457",
      "isCrossProject": null,
      "isDeleted": null,
      "isGlobalScope": null,
      "isRestrictedVisible": null,
      "legacyDescriptor": null,
      "localScopeId": null,
      "mailAddress": null,
      "origin": "vsts",
      "originId": "751219bc-8b64-4e85-b249-7967470a0c49",
      "principalName": "[test from console]\\Contributors",
      "scopeId": null,
      "scopeName": null,
      "scopeType": null,
      "securingHostId": null,
      "specialType": null,
      "subjectKind": "group",
      "url": "https://vssps.dev.azure.com/testtest/_apis/Graph/Groups/vssgp.Uy0xLTktMTU1MTM3NDI0NS0xODAyMTAwNzE1LTI1MzU3MjIwNTItMjMyNjkzMzU3OS0zMjY3NDI5NDYzLTEtNzYxNDIxODkyLTI3NDg1NDQ4NDUtMjk3NzI0MjIzOS0zMTUwMzI1NTI0"
    }

Con el descriptor copiado, ejecutamos el comando:

        az devops security group membership add --group-id descriptor --member-id email_colaborador

## Creamos un repositorio en el proyecto
Por defecto, al crear un proyecto se crea un repositorio con el mismo nombre. Como lo normal en un proyecto es tener más de una única aplicación se tendrá la necesidad de ir creando más repositorios para alojar a cada una de las aplicaciones aunque las mantengamos englobadas bajo el mismo proyecto porque la gestión de los permisos de sus componentes es común.
Vamos a ver cómo crear un nuevo repositorio y borrar uno ya existente. Para realizar estas acciones, se deben hacer con un usuario que tenga los permisos de:
+ Create repository
+ Delete repository

De los grupos de seguridad que se crean al crear el proyecto, este permiso lo tienen:
+ Project Collection Administrators
+ Project Administrators

Estas acciones, por lo tanto, vamos a ejecutarlas con el usuario que creó el proyecto y no con el colaborador que añadimos previamente, ya que al estar en el grupo de Contributors no tendrá permisos.

### Desde la interfaz gráfica
Accedemos a nuestro proyecto de "Code Motion Demo Project" y en el menú lateral izquierdo hacemos click sobre Repos. Al hacerlo, en la parte de arriba, veremos que nos está marcando la ruta donde nos encontramos como:
organizacion / Code Motion Demo Project / Repos / Files / (icono de git) Code Motion Demo Project

Justo a la derecha del nombre del repositorio hay una flecha, que al hacer click sale un desplegable donde se puede ver el listado de repositorios del proyecto y además nos da las opciones de:
+ Crear un repositorio
+ Importar un repositorio
+ Gestionar los repositorios

![Imagen donde se muestra justo lo descrito anteriormente](Imagenes/Opciones_repositorios.PNG)

Tanto desde aquí como desde Gestinar los repositorios podremos crear un nuevo repositorio.
Sólo desde aquí tenemos la opción de clonar un repositorio ya existente.
Sólo desde Gestionar los repositorios podremos renombrar y borrar los repositorios ya existentes.

Seleccionamos entonces Importar repositorio:
+ Tipo de repositorio: Git
+ Url a clonar: la de este mismo repo: https://github.com/TheLadyB/azuredevpos-intro-workshop
+ Nombre: azuredevpos-intro-workshop

Una vez clonado, desde Manage Repositories eliminamos el repositorio que se creó por defecto al crear el proyecto.

### Por línea de comando
Se tiene que hacer en dos pasos: primero se crea el repositorio y posteriormente se importa el contenido en el mismo
       
       az repos create --name azuredevpos-intro-workshop
       az repos import create --git-source-url https://github.com/TheLadyB/azuredevpos-intro-workshop -r azuredevpos-intro-workshop
        
En caso que estuviésemos clonando un repositorio privado, en la instrucción de importación habría que indicar que requiere autorización. Al hacerlo, nos pedirá el PAT del usuario que tiene permisos para acceder al repo privado. Por ejemplo, en caso de que este repositorio fuese privado, el comando sería el seguiente:

       az repos import create --git-source-url https://github.com/TheLadyB/azuredevpos-intro-workshop -r azuredevpos-intro-workshop --requires-authorization --user-name usuario-github

Una vez clonado el repo, eliminamos el que se creó por defecto al crear el proyecto:

       az repos list

Obtenemos el id del repositorio por defecto (tiene el mismo nombre que el proyecto)

       az repos delete --id id-repositorio-defecto

###Inclusión del appsettings.json
Para que el código funcione, es necesario incluir un archivo de appsettings.json al mismo nivel que Program.cs y otro al mismo nivel que DevOpsConnectorTest.cs con la siguiente información:
       
       {
              "Pat": "pat",
              "Organization": "nombre_organizacion",
              "Project": "nombre_proyecto",
              "Usuario": "email usuario dado de alta"
       }


### Creación de una pipeline que compile el código y ejecute las pruebas
Accedemos al proyecto donde tenemos nuestro código y en el menú lateral izquierdo accedemos a la sección de Pipelines y pulsamos el botón de Create Pipeline:

![Imagen donde se muestra el proceso de configuración de la pipeline](Imagenes/Configuracion_Pipeline_Compilacion.gif)

* Elegimos Azure Repo Git como origen de nuestro código
* Elegimos crear una pipeline con estructura básica
* En el archivo yaml que nos aparece para editar lo dejamos de la siguiente manera:

       # Starter pipeline
       # Start with a minimal pipeline that you can customize to build and deploy your code.
       # Add steps that build, run tests, deploy, and more:
       # https://aka.ms/yaml



       pool:
       vmImage: ubuntu-latest

       steps:
       - task: UseDotNet@2
       displayName: 'Install .NET 5.0.x SDK'
       inputs:
       version: '5.0.x'
       performMultiLevelLookup: true
       includePreviewVersions: true # Required for preview versions

       - task: DotNetCoreCLI@2
       displayName: "Compilación del proyecto"
       inputs:
       command: 'build'
       projects: '**/*.csproj'

       - task: DotNetCoreCLI@2
       displayName: "Ejecución de las pruebas"
       inputs:
       command: 'test'
       projects: '**/test.csproj'

* Modificamos el nombre de la pipeline por "Pipeline-compilacion" en lugar del nombre por defecto que nos aparece. 
![](Imagenes/Nombre_Pipeline.PNG)
* Seleccionamos Save and Run para guardar y forzar una ejecución de la pipeline que acabamos de crear

### Creación de una pipeline que genere el artefacto

Repetimos el proceso anterior de compilación de la pipeline con las siguientes diferencias:
* Nombramos a la pipeline como "Pipeline-generadora-artefacto"
* La pipeline es igual a la anterior, pero añadiendo estos steps

       - task: DotNetCoreCLI@2
         inputs:
          command: 'publish'
          publishWebProjects: false
          zipAfterPublish: false
          arguments: '--output $(Build.ArtifactStagingDirectory)'
          projects: '**/AsignacionTareas.csproj'

       - task: PublishBuildArtifacts@1
         inputs:
          pathToPublish: $(Build.ArtifactStagingDirectory)
          ArtifactName: ExNetCoreAppWinService

* Previo a la variable del pool incluimos el trigger (queremos ejecutarlo cuando se suba el código a la rama main)

       trigger:
       - main


###Creamos las ramas
Se pueden crear desde la propia interfaz o mediante línea de comando de git como con cualquier otro repositorio de git. En este caso, vamos a clonar el repositorio y crear las ramas desde la línea de comandos.

* Clonamos el repositorio (incluimos el que hemos creado mediante la interfaz y sobre el que hemos estado trabajando todo el ramo mediante la línea de comandos)

       git clone https://organizacion@dev.azure.com/organizacion/CodeMotion%20WokShop%20roject%20Command%20Line/_git/azuredevpos-intro-workshop && \
       git clone https://organizacion@dev.azure.com/organizacion/Code%20Motion%20Demo%20Project/_git/azuredevpos-intro-workshop

Y desde la ubicación de cada uno de nuestros repositorios, creamos las ramas y las mandamos a origen

       git checkout -b qa
       git push origin master

       git checkout -b develop
       git push origin develop

####Asignamos develop como la default branch
Desde el apartado de branches, posicionamos el ratón sobre la rama de develop y en el menú de tres puntitos que aparece a la derecha seleccionamos "Set as default branch"

###Restringir la estructura de carpetas con la que se pueden crear las ramas
Queremos forzar una nomenclatura y un flujo que deban seguir las ramas en nuestro repositorio. Empezamos forzando que las ramas se creen siguiendo una estructura de carpetas de la siguiente forma:

+ Vamos a forzar a los contribuidores del proyecto a que creen todas sus ramas dentro de una carpeta llamada feature o hotfix

Estas reglas no se pueden implantar mediante la interfaz gráfica, sino sólo dese la cli o desde la consola para desarrolladores del visual studio (pero esto último sólo se podrá ejecutar desde windows y se considera que los comandos tf están deprecados)

Vamos a necesitar los siguientes ids:
+ Descriptor del grupo de Contributors (deberíamos tenerlo de antes, pero en todo caso, lo podemos obtener de nuevo del listado de grupos de seguridad)

       az devops security group list

  + O, desde una consola de powershell

              $project= Nombre_proyecto

              $descriptor_contributor = az devops security group list  | ConvertFrom-Json | select -expand graphGroups | where principalName -eq "[$project]\Project Administrators"

+ Id del repositorio sobre el que queremos aplicar las restricciones. Lo podemos sacar del listado de repositorios

       az repos list

+ Id del proyecto. Lo podemos sacar de la petición de información del proyecto (en este caso, aunque tengamos puesto un proyecto por defecto es obligatorio indicar de qué proyecto concreto queremos obtener la información)

       az devops project   show --project nombre-proyecto

+ Id del namespace de los repositorios de Git dentro de los grupos de permisos
       
       #powershell
       $namespaceId = az devops security permission namespace list --query "[?@.name == 'Git Repositories'].namespaceId | [0]"

       #bash
       namespaceId=$(az devops security permission namespace list --query "[?@.name == 'Git Repositories'].namespaceId | [0]")

+ Construimos en formato hexadecimal el patrón de las ramas que vamos a permitir crear
En una consola powershell

       function hexify($string) {
              return ($string | Format-Hex -Encoding Unicode | Select-Object -Expand Bytes | ForEach-Object { '{0:x2}' -f $_ }) -join ''
       }

       $hexFeatureBranch = hexify -string  "feature"
       $featureToken = "refs/heads/$hexFeatureBranch"

       $hexHotfixBranch = hexify -string  "hotfix"
       $hotfixToken = "refs/heads/$hexHotfixBranch"

En una consola de bash de linux

       str="feature_bash"
       hexFeatureBranch=$(xxd -pu <<< "$str")
       featureToken="refs/heads/$hexFeatureBranch"

       str="hotfix_bash"
       hexHotfixBranch=$(xxd -pu <<< "$str")
       hotfixToken="refs/heads/$hexHotfixBranch"

TODO: ESTO NO LO ESTÁ CODIFICANDO BIEN, PORQUE DEBERÍA TOMARLO COMO UTF-16 Y LO ESTÁ COGIENDO COMO UTF-8


Con toda estos datos ya obtenidos, lo que vamos a hacer es: primero impedir que se puedan crear ramas y luego levantar la restricción para los patrones permitidos:

+ Impedimos que las personas pertenecientes al equipo de Contributors puedan crear cualquier tipo de rama en nuestro repositorio:

       #powershell
       $denytokenbuild = "repoV2/$projectid/$repoid/"
       az devops security permission update --id $namespaceId --subject $descriptor_contributor.descriptor --token $denytokenbuild --deny-bit 16 --allow-bit 16494
       
       #bash
       denytokenbuild="repoV2/$projectid/$repoid/"
       az devops security permission update --id "$namespaceId" --subject $descriptor_contributor.descriptor --token "$denytokenbuild" --deny-bit 16 --allow-bit 16494





+ Creamos una regla para permitir que creen las ramas con un patrón:

       #powershell
       $featureTokenBuild = "repoV2/$projectid/$repoid/$featureToken"
       $hotfixTokenBuild = "repoV2/$projectid/$repoid/$hotfixToken"

       az devops security permission update --id $namespaceId --subject $descriptor_contributor --token $featureTokenBuild --deny-bit 0 --allow-bit 16

       az devops security permission update --id $namespaceId --subject $descriptor_contributor --token $hotfixTokenBuild --deny-bit 0 --allow-bit 16
       
       #bash
       featureTokenBuild="repoV2/$projectid/$repoid/$featureToken"
       hotfixTokenBuild="repoV2/$projectid/$repoid/$hotfixToken"
       
       az devops security permission update --id "$namespaceId" --subject "$descriptor_contributor" --token "$featureTokenBuild" --deny-bit 0 --allow-bit 16

       az devops security permission update --id "$namespaceId" --subject "$descriptor_contributor" --token "$hotfixTokenBuild" --deny-bit 0 --allow-bit 16



###Añadir control de flujo de ramas a la pipeline que compila el código
Ahora que tenemos restringido los nombres que pueden tener las ramas, vamos a controlar en la pipeline si cuando se hace una pullrequest se está siguiendo el flujo que nosotros queremos:
+ Que a la rama de develop sólo se puedan hacer pullrequests desde las ramas de feature o hotfix
+ Que a la rama de qa sólo se puedan hacer pullrequests desde las ramas de develop y hotfix
+ Que a la rama main sólo se puedan hacer pullrequests desde las ramas de qa y hotfix

Dentro de una pipeline tenemos acceso a varias variables, entre ellas, las que nos interesan para este flujo son:
+ System.PullRequest.SourceBranch
+ System.PullRequest.TargetBranch
+ Build.Reason: aunque en principio lo vamos a añadir a la pipeline "Pipeline-compilacion" que luego vamos a configurar para que salte en las pullrequests, vamos a comprobar también que efectivamente la razón por la que se está ejecutando la pipeline es por este motivo.

Accedemos a las pipelines, seleccionamos la pipeline de Pipeline-compilacion, le damos a editar y añadimos una task de Tipo PowerShell. Seleccionamos que se de tipo "Inline" y en el script escribimos lo siguiente:

       $trigger = "$(Build.Reason)"
       if($trigger.ToLower() -eq "pullrequest"){
       $sourceBranch = "$(System.PullRequest.SourceBranch)"
       $targetBranch = "$(System.PullRequest.TargetBranch)"
   
       Write-Host $targetBranch
       Write-Host $sourceBranch
   
       #Target Branch es Main
       if($targetBranch.ToLower().Contains("refs/heads/main") ){
           Write-Output "Target es main"
           if($targetBranch.ToLower().Contains("refs/heads/main") -AND -not($sourceBranch.ToLower().Contains("refs/heads/qa")) -AND -not($sourceBranch.ToLower().Contains("refs/heads/hotfix/")))
           {
               Write-Output "Operacion invalida sobre la master"
               exit 1
           }
       }
       #Target Branch es qa
       if($targetBranch.ToLower().Contains("refs/heads/integracion")){
           Write-Output "Target es QA"
           if($targetBranch.ToLower().Contains("refs/heads/qa") -AND  -not($sourceBranch.ToLower().Contains("refs/heads/develop/")) -AND -not($sourceBranch.ToLower().Contains("refs/heads/hotfix/")) )
           {
               Write-Output "Operacion inválida sobre integracion"
               exit 1
           }
        }
       #Target Branch es develop
       if($targetBranch.ToLower().Contains("refs/heads/develop")){
           Write-Output "Target es Integracion"
           if($targetBranch.ToLower().Contains("refs/heads/develop") -AND  -not($sourceBranch.ToLower().Contains("refs/heads/feature/")) -AND -not($sourceBranch.ToLower().Contains("refs/heads/hotfix/")) )
           {
               Write-Output "Operacion inválida sobre integracion"
               exit 1
           }
        }
       }


En la ErrorActionPreference seleccionamos Stop y la añadimos y guardamos la pipeline.


###Creamos al equipo de "Functional Reviewers"
Vamos a crear un grupo nuevo dentro del equipo al que vamos a denominar "Funcional Reviewers", la idea es que en este grupo estén la personas que deben dar el OK a las pullrequests. En nuestro contexto y dado que estamos trabajando con dos usuarios vamos a incluir al usuario que es administrador en este equipo.
####Interfaz gráfica
Accedemos a la configuración del proyecto, los permisos y seleccionamos crear un nuevo grupo.

![](Imagenes/Creacion_equipo.png)

En el menú de creación añademos directamente al usuario.

####Línea de comandos

       az devops security group create --name 'Functional Reviewers'

Copiamos el descriptor y añadimos al usuario.


###Añadimos políticas a las ramas
Vamos a añadir una política a las ramas de qa, develop y main. Esto hará que no sea posible hacer un push directo a estas ramas, sino que la subida de código tenga que hacerse mediante pull requests. Vamos a poner las siguientes condiciones:

+ Que al menos una persona del grupo de Funcional Reviewers tenga que aprobar la pull request
+ Que la pipeline de Pipeline-compilacion se ejecute correctamente. Por cómo construimos la pipeline esto implicará:
    + Que si no se sigue el flujo definido para las ramas, la pipeline fallará y por lo tanto no podrá completarse la pullrequest
    +  Que no podrá subirse código que no compile
    +  Que no podrá subirse código que no pase las pruebas unitarias definidas

Accedemos a nuestro proyecto, en el apartado del repositorio a las ramas. Y empezando por la rama main, en su menú seleccionamos "branch policies":
+ Añadimos una política de Build Validation:
    + Build pipeline: seleccionamos la pipeline "Pipeline-compilacion"
    + Trigger: Automatic
    + Policy requirement: Required

+ Añadimos una política de "Automatically included reviewers":
    + En reviewers seleccionamos Functional Reviewers
    + Ponemos que sea requerido con un mínimo de 1 revisor (por la limitación de usuarios que tenemos)
    + Desmarcamos que nos podamos aprobar nuestros propios cambios

Repetimos con las ramas de qa y develop


###Añadimos un Resource Group
Lo que vamos a hacer es ejecutar un script en nuestro ordenador local para permitir que nuestras pipelines puedan comunicarse con él para los despliegues..
En nuestro proyecto, en el apartado de pipelines seleccionamos Deployment groups y Add a deployment group. Tenemos que chequear "Use a personal access token in the script for authentication"  y seleccionar el sistema operativo que tengamos en nuestro local.

Copiamos el script y lo ejecutamos.

Una vez finaliza, si accedemos otra vez a Deployment groups podremos ver en el listado el que acabamos de crear con el Target status en Online.

![](Imagenes/Estado_Deployment_Group.PNG)

###Configuramos la pipeline de despliegue
Desde Pipelines, seleccionamos Release y le damos a crear una nueva Pipeline:
+ Como no tenemos ninguna plantilla que se ajuste a lo que queremos (copiar nuestros archivos del artefacto a una carpeta local) seleccionamos Empty job
+ En el menú que aparece, al Stage name lo cambiamos por "Despliegue" y cerramos
+ En el apartado de Artifacts, seleccionamos +Add
  + En source type seleccionamos Build
  + En el proyecto, seleccionamos en el que hemos estado configurando todas las pipelines y demás
  + En Source (build pipeline) seleccionamos la pipeline generadora de artefactos
  + Damos a Add
+ Seleccionamos Tasks
  + Borramos el Agent job que nos aparece (está configurado para un pool y queremos que sea para nuestro Deployment Group)
  + Seleccionando sobre Despliegue, le damos a "Add a deployment group job" y seleccionamos el que hemos creado previamente
  + Añadimos una task a nuestro deployment group: seleccionamos la de "Copy Files To":
    + Configuramos la carpeta de origen (donde se encuentran nuestros artefactos)
    + Configuramos la carpeta de destino: ruta de nuestro local
    + En opciones avanzadas seleccionamos que se reescriban los archivos
+ Guardamos y si queremos probarlo (deberíamos tener un artefacto de cuando creamos la pipeline de generación del artefacto), lanzamos la pipeline


![](Imagenes/Configuracion_Pipeline_Release.gif)

###¿Y ahora qué?

Pues ya lo tenemos todo listo para que al crear una nueva rama de una nueva funcionalidad se tenga que ir subiendo por el flujo de ramas adecuado y que cuando por fin se lleguen a incorporar los cambios en la rama de main, esta de manera automática ejecute la pipeline que genera el artefacto. Al generar el artefacto, la pipeline de release salta (porque su trigger es que exista un artefacto) y nos sustituya el paquete que tenemos en nuestro local.
