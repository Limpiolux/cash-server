# 📘 API - Cash Server

## Descripción General

Esta API expone endpoints para la gestión de empleados (supervisores y preventores), formularios, ítems, subítems, respuestas, usuarios y visitas de servicio.  
Es utilizada por la aplicación de Preventores del área de Cash, cuyo cliente está desarrollado en React.

---

📑 Tabla de Contenidos

### 👥 [EmpleadoController](#empleadocontroller)
1. [Obtener todos los Preventores](#1-obtener-todos-los-preventores)
2. [Obtener todos los Supervisores (LIMPIOLUX)](#2-obtener-todos-los-supervisores-limpiolux)
3. [Obtener correo electrónico del Supervisor por ID](#3-obtener-correo-electrónico-del-supervisor-por-id)
4. [Obtener Supervisores de Ceiling](#4-obtener-supervisores-ceiling)
5. [Obtener Supervisores de FBM](#5-obtener-supervisores-fbm)
6. [Obtener Supervisores de TYT](#6-obtener-supervisores-tyt)
7. [Obtener Supervisores de DistMaster](#7-obtener-supervisores-distmaster)
8. [Obtener Supervisores de Otro Servicio](#8-obtener-supervisores-otro-servicio)

### 📝 [FormularioController](#formulariocontroller)
9. [Obtener Todos los Formularios](#9-obtener-todos-los-formularios)
10. [Listar Tipos de Vehículos](#10-listar-tipos-de-vehículos)
11. [Obtener Formulario por ID](#11-obtener-formulario-por-id)

### 📦 [ItemController](#itemcontroller)
12. [Obtener Items por ID de Formulario](#12-obtener-items-por-id-de-formulario)

### ✅ [RespuestaController](#respuestacontroller)
13. [Obtener Respuestas por ID de Item](#13-obtener-respuestas-por-id-de-item)

### 🏠 [ServicioPrestadoController](#servicioprestadocontroller)
14. [Obtener Servicios de Casas - Limpiolux](#14-obtener-servicios-de-casas---limpiolux)
15. [Obtener Clientes Casa - FBM](#15-obtener-clientes-casa---fbm)
16. [Obtener Casas - TyT y DistMaster](#16-obtener-casas---tyt-y-distmaster)
17. [Obtener Casas - Otro Servicio](#17-obtener-casas---otro-servicio)
18. [Obtener Servicios de Casas - Ceiling](#18-obtener-servicios-de-casas---ceiling)

### 🔧 [SubItemController](#subitemcontroller)
19. [Obtener Subítems por ID de Item](#19-obtener-subítems-por-id-de-item)

### 🏢 [UnidadNegocioController](#unidadnegociocontroller)
20. [Obtener Todas las Unidades de Negocio](#20-obtener-todas-las-unidades-de-negocio)

### 👤 [UserController](#usercontroller)
21. [Registrar Usuario](#21-registrar-usuario)
22. [Iniciar Sesión (login)](#22-iniciar-sesión-login)
23. [Validar Token](#23-validar-token)
24. [Obtener Datos de Usuario](#24-obtener-datos-de-usuario)
25. [Listar Roles de Usuario](#25-listar-roles-de-usuario)
26. [Listar Usuarios Activos](#26-listar-usuarios-activos)
27. [Desactivar Usuario](#27-desactivar-usuario)
28. [Editar Usuario](#28-editar-usuario)
29. [Obtener Rol de Usuario por ID](#29-obtener-rol-de-usuario-por-id)

### 📋 [VisitaServicioController](#visitaserviciocontroller)
30. [Crear Visita de Servicio](#30-crear-visita-de-servicio)
31. [Obtener Todas las Visitas](#31-obtener-todas-las-visitas)
32. [Obtener Visitas por Usuario](#32-obtener-visitas-por-usuario)
33. [Obtener Visitas por Supervisor](#33-obtener-visitas-por-supervisor)
34. [Obtener Visitas de Preventores](#34-obtener-visitas-de-preventores)
35. [Obtener Visitas por Rol de Usuario](#35-obtener-visitas-por-rol-de-usuario)
36. [Filtrar Visitas](#36-filtrar-visitas)
37. [Obtener Visita por ID](#37-obtener-visita-por-id)
38. [Actualizar Datos del Vehículo en una Visita de Servicio](#38-actualizar-datos-del-vehículo-en-una-visita-de-servicio)

### 🧾 [VisitaServicioFormController](#visitaservicioformcontroller)
39. [Crear Formularios de Visita de Servicio](#39-crear-formularios-de-visita-de-servicio)
40. [Obtener Formularios por ID de Visita](#40-obtener-formularios-por-id-de-visita)


## 👥 EmpleadoController

Controlador encargado de gestionar los empleados del sistema, incluyendo preventores y supervisores asociados a distintas unidades de negocio.

## Endpoints

### 1. Obtener todos los Preventores

- **Método:** `GET`
- **URL:** `/empleado/getallpreventores`
- **Descripción:** Devuelve una lista de empleados activos cuyo rol es **Preventor**.
- **Respuesta exitosa (`200 OK`):**
  ```json
  [
    {
      "id": 1,
      "nombre": "Juan Pérez",
      "email": "juan.perez@empresa.com",
      "rol": "Preventor",
      "activo": true
    }
  ]
  ```
- **Errores:**
- 404 Not Found: No se encontraron preventores activos.
- 500 Internal Server Error: Error interno del servidor.

### 2. Obtener Todos los Supervisores (Limpiolux)

- **Método HTTP:** GET
- **Ruta:** /empleado/getallsupervisores
- **Descripción:** Este endpoint obtiene los supervisores de otro endpoint (https://preventores.limpiolux.com.ar:44362), verifica que los supervisores no esten en la tabla, si no están les carga el campo supervisor y activo = true y luego los devuelve para ser leídos posteriormente por el cliente.
- **Respuesta exitosa (`200 OK`):**
```json
  [
    {
      "id": 1,
      "nombre": "Juan Pérez",
      "email": "juan.perez@empresa.com",
      "rol": "Preventor",
      "activo": true
    }
  ]
```
- **Errores:**
- 404 Not Found: No se encontraron supervisores.
- 500 Internal Server Error: Error interno del servidor.

### 3. Obtener Supervisores (Ceiling)

- **Método:** `GET`
- **URL:** `/empleado/getallsupervisoresCeiling`
- **Descripción:** Sincroniza los supervisores desde un servicio externo de la unidad de negocio **Ceiling** (https://serviciosceiling.limpiolux.com.ar:44303).  
  Verifica si existen en la base de datos local. Si no están, los inserta. Si ya existen, los actualiza.  
  Luego retorna la lista completa de supervisores activos de Ceiling.
- **Respuesta exitosa (`200 OK`):**
```json
[
  {
    "id": 11,
    "nombre": "Carlos Gómez",
    "email": "carlos.gomez@ceiling.com",
    "rol": "Supervisor",
    "activo": true
  }
]
```
- **Errores:**
- 404 Not Found: No se encontraron supervisores en el servicio externo.
- 500 Internal Server Error: Error interno del servidor o durante la sincronización.
  
### 4. Obtener correo electrónico del Supervisor por ID

- **Método:** `GET`
- **URL:** `/empleado/getsupervisoremail/{idSupervisor}`
- **Descripción:** Devuelve el correo electrónico del supervisor correspondiente al ID proporcionado.
- **Parámetros:**
- `idSupervisor`: ID entero del supervisor.
- **Respuesta exitosa (`200 OK`):**
  {
    "email": "supervisor@empresa.com"
  }
- **Errores:**
- 404 Not Found: No se encontró un supervisor con ese ID.
- 500 Internal Server Error: Error interno del servidor.

### 5. Obtener Supervisores (FBM)

- **Método:** `GET`
- **URL:** `/empleado/getallsupervisoresFBM`
- **Descripción:** Devuelve la lista de supervisores activos asociados a la unidad de negocio **FBM** (`UnidadNegocio_id = 2`).
- **Respuesta exitosa (`200 OK`):**
```json
[
  {
    "id": 21,
    "nombre": "Laura Fernández",
    "email": "laura.fernandez@fbm.com",
    "rol": "Supervisor",
    "activo": true,
    "unidadNegocio_id": 2
  }
]
```
- **Errores:**
- 404 Not Found: No se encontraron supervisores.
- 500 Internal Server Error: Error interno del servidor.

### 6. Obtener Supervisores (TYT)

- **Método:** `GET`
- **URL:** `/empleado/getallsupervisoresTYT`
- **Descripción:** Devuelve la lista de supervisores activos asociados a la unidad de negocio **TYT** (`UnidadNegocio_id = 3`).
- **Respuesta exitosa (`200 OK`):**
```json
[
  {
    "id": 31,
    "nombre": "Martín López",
    "email": "martin.lopez@tyt.com",
    "rol": "Supervisor",
    "activo": true,
    "unidadNegocio_id": 3
  }
]
```
- **Errores:**
- 404 Not Found: No se encontraron supervisores.
- 500 Internal Server Error: Error interno del servidor.

### 7. Obtener Supervisores (DistMaster)

- **Método:** `GET`
- **URL:** `/empleado/getallsupervisoresDistMaster`
- **Descripción:** Devuelve la lista de supervisores activos asociados a la unidad de negocio **DistMaster** (`UnidadNegocio_id = 4`).
- **Respuesta exitosa (`200 OK`):**
```json
[
  {
    "id": 41,
    "nombre": "Sofía Martínez",
    "email": "sofia.martinez@distmaster.com",
    "rol": "Supervisor",
    "activo": true,
    "unidadNegocio_id": 4
  }
]
```
- **Errores:**
- 404 Not Found: No se encontraron supervisores.
- 500 Internal Server Error: Error interno del servidor.

### 8. Obtener Supervisores (Otro Servicio)

- **Método:** `GET`
- **URL:** `/empleado/getallsupervisoresOtroServicio`
- **Descripción:** Devuelve la lista de supervisores activos asociados a la unidad de negocio **Otro Servicio** (`UnidadNegocio_id = 5`).
- **Respuesta exitosa (`200 OK`):**
```json
[
  {
    "id": 51,
    "nombre": "Diego Ramírez",
    "email": "diego.ramirez@otroservicio.com",
    "rol": "Supervisor",
    "activo": true,
    "unidadNegocio_id": 5
  }
]
```
- **Errores:**
- 404 Not Found: No se encontraron supervisores.
- 500 Internal Server Error: Error interno del servidor.

## Controlador FormularioController

## Endpoints

### 9. Obtener Todos los Formularios

- **Método HTTP:** GET
- **Ruta:** /formulario/getallformularios
- **Descripción:** Este endpoint devuelve una lista de todos los formularios disponibles.
- **Respuestas:**
- 200 OK: La solicitud fue exitosa y se devolvió la lista de formularios.
- 404 Not Found: No se encontraron formularios.
- 500 Internal Server Error: Error interno del servidor.

### 10. Listar Tipos de Vehículos

- **Método HTTP:** GET
- **Ruta:** /formulario/listatipovehiculos
- **Descripción:** Este endpoint devuelve una lista de los tipos de vehículos disponibles, Flota = 1, Alquilado=2
- **Respuestas:**
- 200 OK: La solicitud fue exitosa y se devolvió la lista de tipos de vehículos.
- 500 Internal Server Error: Error interno del servidor.
 
### 11. Obtener Formulario por ID

- **Método HTTP:** `GET`
- **Ruta:** `/formulario/getformulario/{id}`
- **Descripción:** Devuelve un formulario específico según el ID proporcionado.
- **Parámetros:**
- `id` (entero): ID del formulario que se desea obtener.
- **Respuesta exitosa (`200 OK`):**
```json
{
  "id": 7,
  "titulo": "Formulario de Relevamiento",
  "descripcion": "Formulario para inspección de unidades",
  "activo": true
}
```
- **Errores:**
- 404 Not Found: Formulario no encontrado.
- 500 Internal Server Error: Error interno del servidor.
 
## Controlador ItemController

## Endpoints

### 12. Obtener Items por ID de Formulario

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

### 13. Obtener Respuestas por ID de Item

- **Método HTTP:** GET
- **Ruta:** /respuesta/getrespuestasbyitemid/{itemId}
- **Descripción:** Este endpoint devuelve una lista de respuestas pertenecientes a un item específico identificado por su ID.
- **Parámetros:**
- `itemId`: ID del item del que se desean obtener las respuestas.
- **Respuestas:**
- 200 OK: La solicitud fue exitosa y se devolvieron las respuestas del item especificado.
- 404 Not Found: No se encontraron respuestas para el item especificado.
- 500 Internal Server Error: Error interno del servidor.
 
## Controlador ServicioPrestadoController

## Endpoints

### 14. Obtener Servicios de Casas - Limpiolux

- **Método HTTP:** GET
- **Ruta:**  /clientecasa/getallservicioscasasLimpiolux/{unidadNegocioId}
- **Descripción:** Este endpoint sincroniza y devuelve la lista de casas asociadas a servicios prestados de la unidad de negocio Limpiolux. Si no existen en la base local, se insertan; si existen, se actualizan.
- **Parámetros:**
- `unidadNegocioId`: unidadNegocioId: ID de la unidad de negocio (debe ser 1 para Limpiolux).
- **Respuestas:**
- 200 OK: Lista de servicios prestados sincronizados correctamente.
- 404 Not Found: No se encontraron servicios o unidad de negocio inválida.
- 500 Internal Server Error: Error interno del servidor.
 
### 15.  Obtener Clientes Casa - FBM

- **Método HTTP:** GET
- **Ruta:** /clientecasa/getallclientescasaFBM/{unidadNegocioId}
- **Descripción:** Este endpoint sincroniza y devuelve la lista de casas asociadas a la unidad de negocio FBM. Si no existen en la base local, se insertan; si existen, se actualizan.
- **Parámetros:**
- `unidadNegocioId`: ID de la unidad de negocio (debe ser 2 para FBM).
- **Respuestas:**
- 200 OK: Lista de servicios prestados sincronizados correctamente.
- 404 Not Found: No se encontraron servicios o unidad de negocio inválida.
- 500 Internal Server Error: Error interno del servidor.

 ### 16.  Obtener Casas - TyT y DistMaster
 
- **Método HTTP:** GET
- **Ruta:** /clientecasa/getallclientescasaTyTYDistMaster/{unidadNegocioId}
- **Descripción:** Devuelve la lista de casas activas asociadas a la unidad de negocio TyT (ID 3) o DistMaster (ID 4), directamente desde la base de datos local.
- **Parámetros:**
- `unidadNegocioId`: ID de la unidad de negocio (3 para TyT o 4 para DistMaster).
- **Respuestas:**
- 200 OK: Lista de casas encontradas.
- 404 Not Found: No se encontraron casas para la unidad de negocio seleccionada.
- 500 Internal Server Error: Error interno del servidor.

 ### 17.  Obtener Casas - Otro Servicio
 
- **Método HTTP:** GET
- **Ruta:** /clientecasa/getallclientescasaOtroServicio/{unidadNegocioId}
- **Descripción:** Devuelve la lista de casas activas asociadas a la unidad de negocio Otro Servicio (ID 5), directamente desde la base de datos local.
- **Parámetros:**
- `unidadNegocioId`: ID de la unidad de negocio (debe ser 5).
- **Respuestas:**
- 200 OK: Lista de casas encontradas.
- 404 Not Found: No se encontraron casas para la unidad de negocio seleccionada.
- 500 Internal Server Error: Error interno del servidor.
 
### 18.  Obtener Servicios de Casas - Ceiling
 
- **Método HTTP:** GET
- **Ruta:** /clientecasa/getallservicioscasasCeiling/{unidadNegocioId}
- **Descripción:** Este endpoint sincroniza y devuelve la lista de casas asociadas a servicios prestados de la unidad de negocio Ceiling. Si no existen en la base local, se insertan; si existen, se actualizan.
- **Parámetros:**
- `unidadNegocioId`: ID de la unidad de negocio (debe ser 6 para Ceiling).
- **Respuestas:**
- 200 OK: Lista de servicios prestados sincronizados correctamente.
- 404 Not Found: No se encontraron servicios o unidad de negocio inválida.
- 500 Internal Server Error: Error interno del servidor.

## Controlador SubItemController

## Endpoints

### 19. Obtener Subítems por ID de Item

- **Método HTTP:** GET
- **Ruta:** /subitem/getsubitemsbyitemid/{itemId}
- **Descripción:** Este endpoint devuelve una lista de subítems pertenecientes a un item específico identificado por su ID.
- **Parámetros:**
- `itemId`: ID del item del que se desean obtener los subítems.
- **Respuestas:**
- 200 OK: La solicitud fue exitosa y se devolvieron los subítems del item especificado.
- 404 Not Found: No se encontraron subítems para el item especificado.
- 500 Internal Server Error: Error interno del servidor.

## Controlador UnidadNegocioController

## Endpoints

### 20. Obtener Todas las Unidades de Negocio

- **Método HTTP:** GET
- **Ruta:** /unidadnegocio/getallunidadesnegocio
- **Descripción:** Este endpoint devuelve una lista completa de todas las unidades de negocio registradas en el sistema.
- **Respuestas:**
- 200 OK: La solicitud fue exitosa y se devolvió la lista de unidades de negocio.
- 404 Not Found: No se encontraron unidades de negocio en el sistema
- 500 Internal Server Error: Error interno del servidor.
  
 ## Controlador UserController

 ## Endpoints

### 21. Registrar Usuario

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
```
- **Respuestas:**
- 200 OK: Registro exitoso.
- 400 Bad Request: Error en los datos de entrada (errores de validaciones).
- 404 Not Found: No se encontró un preventor con el correo electrónico proporcionado (en caso de rol de preventor).
- 500 Internal Server Error: Error interno del servidor.

### 22. Iniciar Sesión (login)

- **Método HTTP:** POST
- **Ruta:** /user/login
- **Descripción:** Este endpoint permite iniciar sesión en el sistema.
- **Cuerpo de la Solicitud (JSON):**
```json
 {
    "Mail": "mlauri126@gmail.com",
    "Password": "1234"
}
```
- **Respuestas:**
- 200 OK: Inicio de sesión exitoso. Retorna un token JWT.
- 401 Unauthorized: Credenciales incorrectas.
- 404 Not Found: Usuario no encontrado.
- 500 Internal Server Error: Error interno del servidor.

### 23. Validar Token
  
- **Método HTTP:** POST
- **Ruta:** /user/validatetoken
- **Descripción:**  Este endpoint permite validar un token JWT.
- **Cuerpo de la Solicitud (JSON):**
```json
 {
   "Token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6ImRmZ0BleGFtcGxlLmNvbSIsIk1haWwiOiJkZmdAZXhhbXBsZS5jb20iLCJuYmYiOjE3MTEwMzgwNDcsImV4cCI6MTcxMTA0MTY0NywiaWF0IjoxNzExMDM4MDQ3fQ.zOgNg5Njvsx9dQ009AAVLrbS_gbk0arGxF0hKIAvr3E"
}
```
- **Respuestas:**
- 200 OK: Token válido.
- 401 Unauthorized: Se requiere un token de autenticación / Token inválido
- 500 Internal Server Error: Error interno del servidor.

### 24. Obtener Datos de Usuario

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

### 25. Listar Roles de Usuario

- **Método HTTP:** GET
- **Ruta:** /user/listroles
- **Descripción:** Este endpoint permite obtener una lista de roles de usuario válidos.
- **Respuestas:**
- 200 OK: Lista de roles de usuario.
- 500 Internal Server Error: Error interno del servidor.

### 26. Listar Usuarios Activos

- **Método HTTP:** GET
- **Ruta:**  /user/activeusers
- **Descripción:** Este endpoint permite obtener una lista de todos los usuarios activos en el sistema.
- **Respuestas:**
- 200 OK: Lista de usuarios activos.
- 404 Not Found: No se encontraron usuarios activos.
- 500 Internal Server Error: Error interno del servidor.

### 27. Desactivar Usuario

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

### 28. Editar Usuario

- **Método HTTP:** PUT
- **Ruta:** /user/edit/{userId}
- **Descripción:**  Este endpoint permite editar los datos de un usuario existente en el sistema.
- **Parámetros:**
- `userId`: ID del usuario a editar.
- **Cuerpo de la Solicitud (JSON):**
```json
{
   "Name": "Hernan Ingrassia",
   "Mail": "heringrassia@gmail.com",
   "Password": "1234",
   "Rol": "Preventor" / "Administrador"
}
```
- **Respuestas:**
- 200 OK: Usuario actualizado correctamente.
- 400 Bad Request: Error en los datos de entrada.
- 404 Not Found: No se encontró ningún usuario activo con el ID proporcionado.
- 500 Internal Server Error: Error interno del servidor.


### 29. Obtener Rol de Usuario por ID

- **Método HTTP:** GET
- **Ruta:** /user/getuserrole/{userId}
- **Descripción:** Este endpoint permite obtener el rol de un usuario por su ID.
- **Parámetros:**
- `userId`: ID del usuario.
- **Respuestas:**
- 200 OK: Rol del usuario.
- 404 Not Found: No se encontró ningún usuario activo con el ID proporcionado.
- 500 Internal Server Error: Error interno del servidor.


## Controlador VisitaServicioController

 ## Endpoints

### 30. Crear Visita de Servicio

- **Método HTTP:** POST
- **Ruta:** /visitaservicio/crear
- **Descripción:**  Este endpoint permite crear una nueva visita de servicio.
- **Cuerpo de la Solicitud (JSON):**
```json
   {
    "ServicioPrestado": "Reparación de equipo",
    "Cliente": "Empresa ABC",
    "Cliente2": "TEST TEST",
    "UnidadNegocio": "Sucursal 123",
    "FechaVisita": "2024-03-13T10:00:00",
    "ModeloVehiculo": "Toyota Corolla",
    "Conductor": "Juan Pérez",
    "TipoVehiculoId": 1,
    "Dominio": "ABC123",
    "Proveedor": "Taller Mecánico XYZ",
    "SupervisorId": 264,
    "UsuarioId": 3
    "EmailsAdicionales: "pedro@limpiolux.com.ar,andrea@limpiolux.com.ar"
}
```
**Nota:** Cliente2 no es obligatorio puede ir null, EmailsAdicionales es un string en donde los emails van separados por , este campo no es obligatorio.
**Nota:** ModeloVehiculo,Conductor,TipoVehiculoId, Dominio, Proveedor pueden ir en null si si no se está en el formulario de vehiculos.

- **Respuestas:**
- 200 OK: Visita de servicio creada exitosamente. Retorna el ID de la visita.
- 400 Bad Request: El cuerpo de la solicitud no puede estar vacío.
- 500 Internal Server Error: Error interno del servidor.

### 31. Obtener Todas las Visitas

- **Método HTTP:** POST
- **Ruta:** /visitaservicio/visitasgetall
- **Descripción:** Este endpoint devuelve una lista completa de todas las visitas de servicio registradas en el sistema, incluyendo sus formularios asociados.
- **Respuestas:**
- 200 OK: Lista de visitas devuelta exitosamente, incluyendo los formularios relacionados con cada visita.
- 404 Not Found: No se encontraron visitas de servicio registradas.
- 500 Internal Server Error: Error interno del servidor.

### 32. Obtener Visitas por Usuario

- **Método HTTP:** GET
- **Ruta:** /visitaservicio/visitasgetallbyId/{usuarioId}
- **Descripción:** Retorna las visitas asociadas a un usuario activo específico.
- **Parámetros:**
- `usuarioId`: ID del usuario a obtener todas sus visitas.
- **Respuestas:**
- 200 OK: Visitas devueltas exitosamente.
- 400 Bad Request: No hay visitas para el usuario.
- 404 Not Found: El usuario no existe o no está activo.
- 500 Internal Server Error: Error interno del servidor.

### 33. Obtener Visitas por Supervisor

 **Método HTTP:** GET
- **Ruta:** /visitaservicio/visitasgetallbysupervisor/{supervisorId}
- **Descripción:** Retorna las visitas asignadas a un supervisor activo.
- **Parámetros:**
- `supervisorId`: ID del supervisor.
- **Respuestas:**
- 200 OK: Visitas devueltas exitosamente.
- 400 Bad Request: No hay visitas cargadas.
- 404 Not Found: El supervisor no existe o no está activo.
- 500 Internal Server Error: Error interno del servidor.

### 34. Obtener Visitas de Preventores

 **Método HTTP:** GET
- **Ruta:** /visitaservicio/visitasgetallbypreventores
- **Descripción:** Devuelve todas las visitas gestionadas por usuarios con rol de preventor.
- **Respuestas:**
- 200 OK: Visitas devueltas exitosamente.
- 400 Bad Request: No hay visitas o preventores cargados.
- 500 Internal Server Error: Error interno del servidor.

 ### 35. Obtener Visitas por Rol de Usuario

 **Método HTTP:** GET
- **Ruta:** /visitaservicio/visitasgetallbyrol/{rol}
- **Descripción:** DRetorna las visitas gestionadas por usuarios de un rol específico (Administrador o Preventor).
- **Parámetros:**
- `rol`: Rol del usuario (Administrador = 1, Preventor = 2).
- **Respuestas:**
- 200 OK: Visitas devueltas exitosamente.
- 400 Bad Request: No hay usuarios o visitas para el rol especificado.
- 500 Internal Server Error: Error interno del servidor.

 ### 36. Filtrar Visitas

 **Método HTTP:** GET
- **Ruta:** /visitaservicio/filtrar
- **Descripción:** Permite filtrar visitas por múltiples parámetros opcionales.
- **Parámetros opcionales (query string):**
- `unidadNegocioId`: ID de la unidad de negocio.
- `servicioPrestadoId`: ID del servicio prestado.
- `fechaDesde`: Fecha de inicio del rango.
- `fechaHasta`: Fecha de fin del rango.
- `supervisorId`: ID del supervisor.
- `usuarioId`: ID del usuario.
- `supervisorId`: Rol del usuario (Administrador o Preventor).
- **Respuestas:**
- 200 OK: Visitas filtradas devueltas exitosamente.
- 400 Bad Request: Algún parámetro no válido o no hay visitas cargadas.
- 404 Not Found: No se encontraron visitas que coincidan con los filtros.
- 500 Internal Server Error: Error interno del servidor.

### 37. Obtener Visita por ID

 **Método HTTP:** GET
- **Ruta:** /visitaservicio/obtenerVisitaPorId/{id}
- **Descripción:** Devuelve los detalles de una visita específica por su ID.
- **Parámetros:**
- `id`: ID de la visita.
- **Respuestas:**
- 200 OK: Visita encontrada y devuelta exitosamente.
- 404 Not Found: No se encontró la visita con el ID proporcionado.
- 500 Internal Server Error: Error interno del servidor.

### 38. Actualizar Datos del Vehículo en una Visita de Servicio

- **Método HTTP:** PUT  
- **Ruta:** `/visitaservicio/actualizar`  
- **Descripción:** Permite actualizar los datos del vehículo de una visita de servicio existente (modelo, conductor, dominio, tipo de vehículo y proveedor).  
- **Cuerpo de la Solicitud (JSON):**
```json
{
  "Id": 123,
  "Conductor": "Juan Pérez",
  "Dominio": "ABC123",
  "ModeloVehiculo": "Toyota Corolla",
  "TipoVehiculoId": 2,
  "Proveedor": "Proveedor XYZ"
}
```
- **Parámetros opcionales (query string):**
- `id`: ID de la visita a modificar.
- Conductor, Dominio, ModeloVehiculo, TipoVehiculoId, Proveedor: campos a actualizar.
- **Respuestas:**
- 200 OK: La visita fue actualizada exitosamente.
- 400 Bad Request: El cuerpo de la solicitud está vacío.
- 404 Not Found: No se encontró una visita con el ID proporcionado.
- 500 Internal Server Error: Error interno del servidor.

## Controlador VisitaServicioFormController

 ## Endpoints

### 39. Crear Formularios de Visita de Servicio

- **Método HTTP:** POST
- **Ruta:** /visitaservicioform/crear
- **Descripción:** Este endpoint permite registrar uno o varios formularios asociados a una visita de servicio. Procesa imágenes, genera archivos PDF, envía correos con adjuntos y carga datos relevantes en SharePoint si aplica.
- **Cuerpo de la Solicitud (JSON):**
```json
[
  {
    "VisitaId": 7,
    "FormId": 3,
    "Imagen1": "data:image/jpeg;base64,...",
    "Imagen2": "data:image/jpeg;base64,...",
    "Imagen3": "data:image/jpeg;base64,...",
    "Item": "Ejemplo de item 1",
    "SubItem": "Ejemplo de subitem 1",
    "Comentario": "Ejemplo de comentario 1",
    "Respuesta": "Ejemplo de respuesta 1",
    "Version": 16,
    "ComentarioGeneral": "Ejemplo"
  },
  {
    "VisitaId": 7,
    "FormId": 2,
    "Imagen1": "data:image/jpeg;base64,...",
    "Imagen2": "data:image/jpeg;base64,...",
    "Imagen3": "data:image/jpeg;base64,...",
    "Item": "Ejemplo de item 2",
    "SubItem": "Ejemplo de subitem 2",
    "Comentario": null,
    "Respuesta": "Ejemplo de respuesta 2",
    "Version": 16,
    "ComentarioGeneral": "Ejemplo"
  }
]
```
**Nota:** 
- El campo Comentario puede ser null.
- Las imágenes deben estar codificadas en base64.
- Las imágenes y respuestas específicas se usan para enviar reportes por correo y cargar datos en SharePoint según reglas de negocio predefinidas.
- El envío de correos se dirige a los supervisores, junto con emails adicionales configurados en la visita.
- **Respuestas:**
- 200 OK: Formularios creados y procesados exitosamente. Devuelve "OK".
- 400 Bad Request: La lista de formularios está vacía o nula.
- 500 Internal Server Error: Error interno del servidor.

### 40. Obtener Formularios por ID de Visita

- **Método HTTP:** GET
- **Ruta:** /visitaservicioform/obtenerformulariosByIdVisita/{idVisita}
- **Descripción:** Devuelve la lista de formularios cargados para una visita específica.
- **Parámetros:**
- `idVisita`: ID de la visita de la cual se desea obtener los formularios.
- **Respuestas:**
- 200 OK: Lista de formularios devuelta exitosamente.
- 400 Bad Request: La visita con el ID especificado no existe.
- 500 Internal Server Error: Error interno del servidor.
  


  



  

  



 


 
    
 
  
 
  
