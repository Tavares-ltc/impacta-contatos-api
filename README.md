
# ABOUT

The main idea of ​​this API is to supply the necessary data for the proper functioning of the front end application, which has the premise of creating, searching and filtering contacts.  
  
   Front-End: https://github.com/Tavares-ltc/impacta-contatos-web

# HOW TO RUN


1. Clone this repository

2. Create configure mongo connection in appsettings.json as you wish

3. dotnet restore

4. dotnet ef database update -Context IdentityContext

5. dotnet ef database update -Context ApplicationDbContext

6. dotnet run (OR) Run the Solution using Visual Studio

- Other option is to exec the database and API with Docker Compose.

1. go to the root of the project

2. docker-compose up

### To mock some data use:

#### POST /api/Contact/mock

# Swagger

You can view endpoints with swagger
![photo_2023-12-25_23-18-36](https://github.com/Tavares-ltc/impacta-contatos-api/assets/98609823/faba8d94-3f07-463c-9e2a-315fc61c0bea)


 
# impacta-contatos-api
## Version: 1.0

### /api/Contact

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| pageNumber | query |  | yes | integer |
| pageSize | query |  | yes | integer |
| sortOrder | query |  | yes | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

#### POST
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

#### PUT
##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

#### DELETE
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| contactId | query |  | yes | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Contact/find

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| field | query |  | yes | string |
| value | query |  | yes | string |
| pageNumber | query |  | no | integer |
| pageSize | query |  | no | integer |
| sortOrder | query | ascending or descending (by createdAt)  | no | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Contact/findByDate

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| dateTime | query |  | yes | dateTime |
| pageNumber | query |  | No | integer |
| pageSize | query |  | No | integer |
| sortOrder | query |  | No | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Contact/search

#### GET
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| searchString | query |  | yes | string |
| pageNumber | query |  | No | integer |
| pageSize | query |  | No | integer |
| sortOrder | query |  | No | string |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |

### /api/Contact/mock

#### POST
##### Parameters

| Name | Located in | Description | Required | Schema |
| ---- | ---------- | ----------- | -------- | ---- |
| numberOfContacts | query | mock some data | No | integer |

##### Responses

| Code | Description |
| ---- | ----------- |
| 200 | Success |
