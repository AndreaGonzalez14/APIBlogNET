# API RESTfull Blog

Configuración de un Proyecto con .NET y Entity Framewrok para una API RESTful de un Blog

# Herramientas

- .Net SDK 8.0.406
- Visual Studio
- XAMPP
- MySQL

# Paquetes

- Entity Framework Core 8.0.13
- Entity Framework Tool 8.0.13
- Microsoft VisualStudio CodeGeneration Design (Scaffolding)
- Pomelo Entity Framework 8.0.3

# Configuración

Inicialmente, el proyecto se creó en **Visual Studio** utilizando la interfaz gráfica y seleccionando la plantilla Web API, lo que genera la estructura base del proyecto. Alternativamente, también se puede crear mediante la línea de comandos ejecutando:

```
dotnet new webapi -n MiApi

```

Después, es necesario instalar los paquetes requeridos. En Visual Studio, esto se puede hacer a través del **Administrador de paquetes NuGet** o, alternativamente, mediante la consola de comandos.

Ejemplo de instalación de paquetes por consola:
```
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.Design

```

⚠️ **¡IMPORTANTE!** 

Es importante asegurarse de que las versiones de los paquetes sean compatibles entre sí para evitar problemas de compatibilidad.

---

Una vez que hemos definido los modelos para nuestra API, el siguiente paso es crear el DbContext, que nos permitirá interactuar con la base de datos.
Ejemplo de DBContext:

```
namespace FirstAPINet
{
    public class ApplicationDbContext:DbContext
    {
        //Constructor
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options){}
        public DbSet<Comment> Comments { get; set; }
    }
}

```

Además, es necesario registrar el DbContext en `Program.cs` y agregar la cadena de conexión en `appsettings.json` para establecer la conexión con la base de datos.

Cadena de Conexión para MySQL
```
Server=localhost;Database=NetFirstBlog;uid=root;Pwd=

```
Se ejecuta las migraciones con los comandos:
```
Add-Migrations NOMBRE_MIGRACION
Update-database

```

Finalmente, se crean los controladores para definir los endpoints de la API. Estos endpoints pueden ser fácilmente probados utilizando Swagger, que viene integrado en el proyecto. Swagger es una herramienta muy útil que facilita la prueba y documentación de los endpoints, permitiendo interactuar con la API de manera visual y sencilla.

🚨 **¡PRECAUCIÓN!** 

Si la API se va a probar localmente y se conectará con un frontend, es importante configurar CORS (Cross-Origin Resource Sharing).

CORS es un mecanismo de seguridad en los navegadores que restringe las solicitudes HTTP realizadas desde un origen diferente al del servidor. Si no se habilita correctamente, el frontend no podrá comunicarse con la API.

La recomendación es habilitar CORS en el backend dentro de Program.cs, de la siguiente manera:
```
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
        builder.AllowAnyOrigin()   // Permite cualquier origen
               .AllowAnyMethod()   // Permite cualquier método (GET, POST, etc.)
               .AllowAnyHeader()); // Permite cualquier encabezado
});


```
