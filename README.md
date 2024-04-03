# Documentación de la API

## Descripción General

La API proporciona endpoints para realizar operaciones relacionadas con empleados (supervisores y preventores), formularios, visitas de servicios etc.

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
 
  
