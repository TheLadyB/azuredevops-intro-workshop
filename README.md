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
![Página de nuestra nueva organización donde se nos pide el nombre de un nuevo proyecto a crear y si queremos que su visibilidad sea pública o privada](Imagenes/Nuevo_Proyecto.PNG). Escrimos un nombre, seleccionamos como visibilidad Private y damos al botón de crear un nuevo proyecto.

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

