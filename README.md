# Documentación de la API - Cash Server

## Descripción General

La API proporciona endpoints para realizar operaciones relacionadas con gestión de empleados (supervisores y preventores), gestión de formularios, items, subitems, respuestas, gestión de usuarios y visitas de servicios.

## Tabla de Contenidos

1. [Obtener Todos los Preventores](#1-obtener-todos-los-preventores)
2. [Obtener Todos los Supervisores](#2-obtener-todos-los-supervisores)
3. [Obtener Correo Electrónico del Supervisor por ID](#3-obtener-correo-electrónico-del-supervisor-por-id)
4. sdfsf
5. sdfsf
6. fsfds
7. fsdf
8. sfsfd

## Controlador EmpleadoController

## Endpoints

### 1. Obtener Todos los Preventores

- **Método HTTP:** GET
- **Ruta:** /empleado/getallpreventores
- **Descripción:** Este endpoint devuelve una lista de preventores activos, es decir los empleados que son preventores que tienen RolEmpleado, Preventor= 1
- **Respuestas:**
  - 200 OK: La solicitud fue exitosa y se devolvió la lista de preventores.
  - 404 Not Found: No se encontraron preventores activos.
  - 500 Internal Server Error: Error interno del servidor.

### 2. Obtener Todos los Supervisores

- **Método HTTP:** GET
- **Ruta:** /empleado/getallsupervisores
- **Descripción:** Este endpoint obtiene los supervisores de otro endpoint, verifica que los supervisores no esten en la tabla, si no están les carga el campo supervisor y activo = true y luego los devuelve para ser leídos posteriormente por el cliente.
- **Respuestas:**
  - 200 OK: La solicitud fue exitosa y se devolvieron los supervisores actualizados.
  - 404 Not Found: No se encontraron supervisores.
  - 500 Internal Server Error: Error interno del servidor.

### 3. Obtener Correo Electrónico del Supervisor por ID

- **Método HTTP:** GET
- **Ruta:** /empleado/getsupervisoremail/{idSupervisor}
- **Descripción:** Este endpoint devuelve el correo electrónico de un supervisor por su ID.
- - **Parámetros:**
  - `idSupervisor`: ID del supervisor del que se desea obtener el correo electrónico
- **Respuestas:**
  - 200 OK: La solicitud fue exitosa y se devolvió el correo electrónico del supervisor.
  - 404 Not Found: No se encontró ningún supervisor con el ID proporcionado.
  - 500 Internal Server Error: Error interno del servidor.
 
## Controlador FormularioController

## Endpoints

### 4. Obtener Todos los Formularios

- **Método HTTP:** GET
- **Ruta:** /formulario/getallformularios
- **Descripción:** Este endpoint devuelve una lista de todos los formularios disponibles.
- **Respuestas:**
  - 200 OK: La solicitud fue exitosa y se devolvió la lista de formularios.
  - 404 Not Found: No se encontraron formularios.
  - 500 Internal Server Error: Error interno del servidor.

### 5. Listar Tipos de Vehículos

- **Método HTTP:** GET
- **Ruta:** /formulario/listatipovehiculos
- **Descripción:** Este endpoint devuelve una lista de los tipos de vehículos disponibles, Flota = 1, Alquilado=2
- **Respuestas:**
  - 200 OK: La solicitud fue exitosa y se devolvió la lista de tipos de vehículos.
  - 500 Internal Server Error: Error interno del servidor.
 
## Controlador ItemController

## Endpoints

### 6. Obtener Items por ID de Formulario

- **Método HTTP:** GET
- **Ruta:** /item/getitemsbyformid/{formId}
- **Descripción:** Este endpoint devuelve una lista de items pertenecientes a un formulario específico identificado por su ID.
- **Parámetros:**
  - `formId`: ID del formulario del que se desean obtener los items.
- **Respuestas:**
  - 200 OK: La solicitud fue exitosa y se devolvieron los items del formulario especificado.
  - 404 Not Found: No se encontraron items para el formulario especificado.
  - 500 Internal Server Error: Error interno del servidor.
 
## Controlador RespuestaController

## Endpoints

### 7. Obtener Respuestas por ID de Item

- **Método HTTP:** GET
- **Ruta:** /respuesta/getrespuestasbyitemid/{itemId}
- **Descripción:** Este endpoint devuelve una lista de respuestas pertenecientes a un item específico identificado por su ID.
- **Parámetros:**
  - `itemId`: ID del item del que se desean obtener las respuestas.
- **Respuestas:**
  - 200 OK: La solicitud fue exitosa y se devolvieron las respuestas del item especificado.
  - 404 Not Found: No se encontraron respuestas para el item especificado.
  - 500 Internal Server Error: Error interno del servidor.
 
## Controlador SubItemController

## Endpoints

### 8. Obtener Subítems por ID de Item

- **Método HTTP:** GET
- **Ruta:** /subitem/getsubitemsbyitemid/{itemId}
- **Descripción:** Este endpoint devuelve una lista de subítems pertenecientes a un item específico identificado por su ID.
- **Parámetros:**
  - `itemId`: ID del item del que se desean obtener los subítems.
- **Respuestas:**
  - 200 OK: La solicitud fue exitosa y se devolvieron los subítems del item especificado.
  - 404 Not Found: No se encontraron subítems para el item especificado.
  - 500 Internal Server Error: Error interno del servidor.

 ## Controlador UserController

 ## Endpoints

### 9. Registrar Usuario

Notas: 
El usuario tiene, si es preventor:
Name: se saca del select del preventor, tomando el nombre (el select preventor se rellena con un endpoint con los datos del preventor)
Mail: se saca del select del preventor, tomando el mail. En el select de preventor se debe concatenar email - nombre
Password: se saca del campo de texto 
Rol: se saca del select de Rol, a Rol se le pasa el texto Preventor, proveniente del select.

El Usuario si es Administrador, va a tener:
Name: se saca del cuadro de texto donde se escribe Nombre y apellido
Email: se saca del texto donde se escribe el Email
Password: se saca del campo de texto donde se escribe el pass
Rol: se saca del select, se le pasaría el texto del select en este caso Administrador.

- **Método HTTP:** POST
- **Ruta:** /user/register
- **Descripción:** Este endpoint permite registrar un nuevo usuario en el sistema.
- **Cuerpo de la Solicitud (JSON):**
  ```json
  {
    "Name": "Hernán Ingrassia",
    "Mail": "heringrassia@gmail.com",
    "Password": "1234",
    "Rol": "Preventor o Administrador"
}
- **Respuestas:**
- 200 OK: Registro exitoso.
- 400 Bad Request: Error en los datos de entrada (errores de validaciones).
- 404 Not Found: No se encontró un preventor con el correo electrónico proporcionado (en caso de rol de preventor).
- 500 Internal Server Error: Error interno del servidor.

### 10. Iniciar Sesión

- **Método HTTP:** POST
- **Ruta:** /user/login
- **Descripción:** Este endpoint permite iniciar sesión en el sistema.
- **Cuerpo de la Solicitud (JSON):**
  ```json
 {
    "Mail": "mlauri126@gmail.com",
    "Password": "1234"
}
- **Respuestas:**
- 200 OK: Inicio de sesión exitoso. Retorna un token JWT.
- 401 Unauthorized: Credenciales incorrectas.
- 404 Not Found: Usuario no encontrado.
- 500 Internal Server Error: Error interno del servidor.

### 11. Validar Token
  
- **Método HTTP:** POST
- **Ruta:** /user/validatetoken
- **Descripción:**  Este endpoint permite validar un token JWT.
- **Cuerpo de la Solicitud (JSON):**
  ```json
 {
   "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImRmZ0BleGFtcGxlLmNvbSIsIk1haWwiOiJkZmdAZXhhbXBsZS5jb20iLCJuYmYiOjE3MTEwMzgwNDcsImV4cCI6MTcxMTA0MTY0NywiaWF0IjoxNzExMDM4MDQ3fQ.zOgNg5Njvsx9dQ009AAVLrbS_gbk0arGxF0hKIAvr3E"
}
- **Respuestas:**
- 200 OK: Token válido.
- 401 Unauthorized: Se requiere un token de autenticación / Token inválido
- 500 Internal Server Error: Error interno del servidor.

### 12. Obtener Datos de Usuario

- **Método HTTP:** GET
- **Ruta:** /user/getuserdata
- **Descripción:**  Este endpoint permite obtener los datos de un usuario autenticado.
- **Headers:**
  Key: Authorization
  Value: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6Im1sYXVyaTEyNkBnbWFpbC5jb20iLCJNYWlsIjoibWxhdXJpMTI2QGdtYWlsLmNvbSIsIm5iZiI6MTcxMTExOTE2MywiZXhwIjoxNzExMTIyNzYzLCJpYXQiOjE3MTExMTkxNjN9.JqBdZJ2JgzG0N_WuJ5HgNiqYix-e7AcLYDdybSryBg4
- **Respuestas:**
- 200 OK: Datos del usuario.
- 401 Unauthorized: Token de autenticación no proporcionado o inválido.
- 404 Not Found: No se encontró ningún usuario asociado con el token proporcionado.
- 500 Internal Server Error: Error interno del servidor.

### 13. Listar Roles de Usuario

- **Método HTTP:** GET
- **Ruta:** /user/listroles
- **Descripción:** Este endpoint permite obtener una lista de roles de usuario válidos.
- **Respuestas:**
- 200 OK: Lista de roles de usuario.

### 14. Obtener Rol de Usuario por ID

- **Método HTTP:** GET
- **Ruta:** /user/getuserrole/{userId}
- **Descripción:** Este endpoint permite obtener el rol de un usuario por su ID.
- **Parámetros:**
  - `userId`: ID del usuario.
- **Respuestas:**
- 200 OK: Rol del usuario.
- 404 Not Found: No se encontró ningún usuario activo con el ID proporcionado.
- 500 Internal Server Error: Error interno del servidor.

### 15. Listar Usuarios Activos

- **Método HTTP:** GET
- **Ruta:**  /user/activeusers
- **Descripción:** Este endpoint permite obtener una lista de todos los usuarios activos en el sistema.
- **Respuestas:**
- 200 OK: Lista de usuarios activos.
- 404 Not Found: No se encontraron usuarios activos.
- 500 Internal Server Error: Error interno del servidor.

### 16. Desactivar Usuario

- **Método HTTP:** PUT
- **Ruta:**  /user/delete/{userId}
- **Descripción:** Este endpoint permite desactivar un usuario existente en el sistema.
- **Parámetros:**
- `userId`: ID del usuario a desactivar.
- **Respuestas:**
- 200 OK: Usuario desactivado correctamente.
- 400 Bad Request: No se puede eliminar el usuario porque tiene visitas asociadas.
- 404 Not Found: No se encontró ningún usuario con el ID proporcionado.
- 500 Internal Server Error: Error interno del servidor.

### 17. Editar Usuario

- **Método HTTP:** PUT
- **Ruta:** /user/edit/{userId}
- **Descripción:**  Este endpoint permite editar los datos de un usuario existente en el sistema.
- **Parámetros:**
- `userId`: ID del usuario a editar.
- - **Cuerpo de la Solicitud (JSON):**
  ```json
{
   "Name": "Hernan Ingrassia",
   "Mail": "heringrassia@gmail.com",
   "Password": "1234",
   "Rol": "Preventor" / "Administrador"
}
- **Respuestas:**
- 200 OK: Usuario actualizado correctamente.
- 400 Bad Request: Error en los datos de entrada.
- 404 Not Found: No se encontró ningún usuario activo con el ID proporcionado.
- 500 Internal Server Error: Error interno del servidor.


## Controlador VisitaServicioController

 ## Endpoints

### 18. Crear Visita de Servicio

- **Método HTTP:** POST
- **Ruta:** /visitaservicio/crear
- **Descripción:**  Este endpoint permite crear una nueva visita de servicio.
- **Parámetros:**
- `userId`: ID del usuario a editar.
- - **Cuerpo de la Solicitud (JSON):**
  ```json
   {
    "ServicioPrestado": "Reparación de equipo",
    "Cliente": "Empresa ABC",
    "UnidadNegocio": "Sucursal 123",
    "FechaVisita": "2024-03-13T10:00:00",
    "ModeloVehiculo": "Toyota Corolla",
    "Conductor": "Juan Pérez",
    "TipoVehiculoId": 1,
    "Dominio": "ABC123",
    "Proveedor": "Taller Mecánico XYZ",
    "SupervisorId": 264,
    "UsuarioId": 3
}
Nota: ModeloVehiculo,Conductor,TipoVehiculoId, Dominio, Proveedor pueden ir en null si si no se está en el formulario de vehiculos.

- **Respuestas:**
- 200 OK: Visita de servicio creada exitosamente. Retorna el ID de la visita.
- 400 Bad Request: El cuerpo de la solicitud no puede estar vacío.
- 500 Internal Server Error: Error interno del servidor.

  ### 19. Obtener Todas las Visitas


  

  

  

  


  



  

  



 


 
    
 
  
 
  
