# Acme LTDA
## Comenzando 🚀

### Pre-requisitos 📋
_Luego de clonar el repositorio:_
_¿Qué cosas se necesitan para instalar el software de manera local?_
* [dotnet](https://dotnet.microsoft.com/download/thank-you/dotnet-sdk-3.0.100-windows-x64-installer) - Sdk para utilizar .Net Core
* Sql Server (Opcional)

Se puede comprobar la instalación en la carpeta del proyecto **WebApi** con el comando
```
dotnet --version
```

### Migraciones 🔧
* _Ubicarse en la carpeta **Infrastructure** y ejecutar los siguientes comandos_
```
dotnet ef migrations add Initial --context=BancoContext -s ../WebApi/
dotnet ef database update --context=BancoContext -s ../WebApi/ 
```
---
⌨️ con ❤️ basado en  [Villanuevand](https://gist.github.com/Villanuevand/6386899f70346d4580c723232524d35a#file-readme-espanol-md) 😊
