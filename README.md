# azuredevpos-intro-workshop

## Preparando lo necesario

### Cuenta de Azure DevOps donde vamos a trabajar
+ Tener una cuenta de outlook (de las normales, valen las gratuitas) [Puedes crear una aquí](https://outlook.live.com/owa/) o una cuenta de Github
+ [Crear una organización en Azure DevOps con la cuenta de outlook](https://azure.microsoft.com/en-us/services/devops/)
    + Selecciona Start free si tienes cuenta de Outlook
    + Selecciona Start fee with GitHub si tienes cuenta de GitHub: deberás autorizar a que Microsoft tenga acceso a tu cuenta y completar el proceso de verificación de que efectivamente eres tú

Una vez completado el proceso de crear la organización en Azure DevOps, os remitirá a una url del tipo https://dev.azure.com/nombreOrganizacion . Si olvidáis el nombre de la organización, siempre podéis volver a la url donde creasteis la organización (https://azure.microsoft.com/en-us/services/devops/) y desde ahí seleccionar "Sign in to Azure DevOps" para que os lleve a vuestra organización

### Código y Framework
En la práctica vamos a trabajar con un proyecto desarrollado en [.NET 5](https://dotnet.microsoft.com/download) y los pantallazos que me veréis del IDE serán de [Visual Studio Code](https://code.visualstudio.com/)

Evidentemente, no es obligatorio ni usar mi proyecto de ejemplo, ni trabajar con ese lenguaje, ni con ese IDE. Mi elección se basa tanto en que a mí me resultan familiares y cómodos como en que son multiplataforma.

#### Instalación de .Net Core
+ Windows: Se descarga el instalable de [.NET 5](https://dotnet.microsoft.com/download)
+ Linux: En la mayoría de las distribuciones se puede [instalar a través del package manager](https://docs.microsoft.com/en-us/dotnet/core/install/linux). Otra opción es descargarse los binarios de [aquí](https://dotnet.microsoft.com/download/dotnet/5.0) y realizar la instalación manualmente

#### Instalación de Visual Studio Code
Desde su [página](https://code.visualstudio.com/download) se puede descargar el instalador para Windows y los paquetes para Linux

## Creando nuestro primer proyecto y añadiendo a colaboradores
