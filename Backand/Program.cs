using Backand;
using Backand.ManagersClasses;
using Microsoft.AspNetCore.Authorization;


var builder = WebApplication.CreateBuilder();
string MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: MyAllowSpecificOrigins,
                      policy =>
                      {
                          policy.AllowAnyMethod();
                          policy.AllowAnyHeader();
                          policy.AllowAnyOrigin();
                      });
});
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddDbContext<ApplicationContext>();
var app = builder.Build();
app.UseCors(MyAllowSpecificOrigins);

//CRUD FOR MINES
app.MapGet("/field", FieldManagers.GetAllMines);
app.MapGet("/field/{id}",FieldManagers.GetMineById);
app.MapDelete("/field/{id}", FieldManagers.DeleteMine);
app.MapPost("/field",FieldManagers.CreateMine);
app.MapPut("/field", FieldManagers.UpdateMine);

//CRUD FOR OBJECTS
app.MapGet("/objects", ObjectsManagers.GetAllObjects);
app.MapGet("/objects/byMine/{mine_id:int}", ObjectsManagers.GetObjectsByMineId);
app.MapGet("/object/{id}", ObjectsManagers.GetObjectById);
app.MapDelete("/object/{id}", ObjectsManagers.DeleteObject);
app.MapPost("/objects", ObjectsManagers.CreateObject);
app.MapPut("/objects", ObjectsManagers.UpdateObject);


//CRUD FOR CONSTRUCTION 
app.MapGet("/constructions", ConstructionManagers.GetAllConstructions);
app.MapGet("/constructions/{object_id:int}", ConstructionManagers.GetConstructionsByObjectId);
app.MapGet("/constructions/planned", ConstructionManagers.GetPlannedConstructions);
app.MapGet("/construction/{construction_id:int}", ConstructionManagers.GetConstructionById);
app.MapDelete("/construction/{id}", ConstructionManagers.DeleteConstruction);
app.MapPost("/construction", ConstructionManagers.CreateConstruction);
//app.MapPut("/constructions", ConstructionManagers.UpdateConstruction);


//CRUD FOR ConstructionUnitType
app.MapGet("/construction_unit_type", ConstructionUnitTypeManagers.GetAllConstructionUnitType);
app.MapGet("/construction_unit_type/{id}", ConstructionUnitTypeManagers.GetByConstructionUnitTypeId);
app.MapDelete("/construction_unit_type/{id}", ConstructionUnitTypeManagers.DeleteConstructionUnitType);
app.MapPost("/construction_unit_type", ConstructionUnitTypeManagers.CreateConstructionUnitType);
app.MapPut("/construction_unit_type", ConstructionUnitTypeManagers.UpdateConstructionUnitType);

//CRUD FOR CONSTRUCTION UNIT
app.MapGet("/construction_unit", ConstructionUnitManagers.GetAllConstructionUnit);
app.MapGet("/construction_unit/{id}", ConstructionUnitManagers.GetConstructionUnitById);
app.MapDelete("/construction_unit/{id}", ConstructionUnitManagers.DeleteConstructionUnit);
app.MapPost("/construction_unit", ConstructionUnitManagers.CreateConstructionUnit);
app.MapPut("/construction_unit", ConstructionUnitManagers.UpdateConstructionUnit);

//CRUD FOR MATERIAL SET
app.MapGet("/material_set", MaterialSetManagers.GetAllMaterialSet);
app.MapGet("/material_set/{id}", MaterialSetManagers.GetMaterialSetById);
app.MapDelete("/material_set/{id}", MaterialSetManagers.DeleteMaterialSet);
app.MapPost("/material_set", MaterialSetManagers.CreateMaterialSet);
app.MapPut("/material_set", MaterialSetManagers.UpdateMaterialSet);

//CRUD FOR MaterialSet_ConstructionUnit
app.MapGet("/material_set_construction_unit", MaterialSet_ConstructionUnitManagers.GetAllMaterial_SetConstructionUnit);
app.MapGet("/material_set_construction_unit/{id}", MaterialSet_ConstructionUnitManagers.GetMaterialSet_ConstructionUnitById);
app.MapDelete("/material_set_construction_unit/{id}", MaterialSet_ConstructionUnitManagers.DeleteMaterialSet_ConstructionUnit);
app.MapPost("/material_set_construction_unit", MaterialSet_ConstructionUnitManagers.CreateMaterialSet_ConstructionUnit);
app.MapPut("/material_set_construction_unit", MaterialSet_ConstructionUnitManagers.UpdateMaterialSet_ConstructionUnit);

//CRUD FOR MANUFACTURE 
app.MapGet("/manufacturer", ManufactureManagers.GetAllManufacture);
app.MapGet("/manufacturer/{id}", ManufactureManagers.GetManufactureById);
app.MapDelete("/manufacturer/{id}", ManufactureManagers.DeleteManufacture);
app.MapPost("/manufacturer", ManufactureManagers.CreateManufacture);
app.MapPut("/manufacturer", ManufactureManagers.UpdateManufacture);

//CRUD FOR LOGISTIS COMPANY 
app.MapGet("/logistic_company", LogisticCompanyManagers.GetAllLogisticCompany);
app.MapGet("/logistic_company/{id}", LogisticCompanyManagers.GetLogisticCompanyById);
app.MapDelete("/logistic_company/{id}", LogisticCompanyManagers.DeleteLogisticCompany);
app.MapPost("/logistic_company", LogisticCompanyManagers.CreateLogisticCompany);
app.MapPut("/logistic_company", LogisticCompanyManagers.UpdateLogisticCompany);

//CRUD FOR STORAGE
app.MapGet("/storage", StorageManagers.GetAllStorage);
app.MapGet("/storage/{id}", StorageManagers.GetStorageById);
app.MapDelete("/storage/{id}", StorageManagers.DeleteStorage);
app.MapPost("/storage", StorageManagers.CreateStorage);
app.MapPut("/storage", StorageManagers.UpdateStorage);

//CRUD FOR Storage_ConstructionUnit
app.MapGet("/storage_construction_unit", Storage_ConstructionUnitManagers.GetAllStorage_ConstructionUnit);
app.MapGet("/storage_construction_unit/{id}", Storage_ConstructionUnitManagers.GetStorage_ConstructionUnitById);
app.MapDelete("/storage_construction_unit/{id}", Storage_ConstructionUnitManagers.DeleteStorage_ConstructionUnit);
app.MapPost("/storage_construction_unit", Storage_ConstructionUnitManagers.CreateStorage_ConstructionUnit);
app.MapPut("/storage_construction_unit", Storage_ConstructionUnitManagers.UpdateStorage_ConstructionUnit);

//CRUD FOR COMPANY TYPE
app.MapGet("/company_type", CompanyTypeManagers.GetAllCompanyType);
app.MapGet("/company_type/{id}", CompanyTypeManagers.GetCompanyTypeById);
app.MapDelete("/company_type/{id}", CompanyTypeManagers.DeleteCompanyType);
app.MapPost("/company_type", CompanyTypeManagers.CreateCompanyType);
app.MapPut("/company_type", CompanyTypeManagers.UpdateCompanyType);

/*//CRUD FOR CONTACT INFO
app.MapGet("/contact_info_type", ContactInfoTypeManagers.GetAllContactInfoType);
app.MapGet("/contact_info_type/{id}", ContactInfoTypeManagers.GetContactInfoTypeById);
app.MapDelete("/contact_info_type/{id}", ContactInfoTypeManagers.DeleteContactInfoType);
app.MapPost("/contact_info_type", ContactInfoTypeManagers.CreateContactInfoType);
app.MapPut("/contact_info_type", ContactInfoTypeManagers.UpdateContactInfoType);*/

//CRUD FOR COMPANY
app.MapGet("/company", CompanyManagers.GetAllCompany);
app.MapGet("/company/{id}", CompanyManagers.GetCompanyById);
app.MapDelete("/company/{id}", CompanyManagers.DeleteCompany);
app.MapPost("/company", CompanyManagers.CreateCompany);
app.MapPut("/company", CompanyManagers.UpdateCompany);

//CRUD FOR TransportType
app.MapGet("/transport_type", TransportTypeManagers.GetAllTransportType);
app.MapGet("/transport_type/{id}", TransportTypeManagers.GetTransportTypeById);
app.MapDelete("/transport_type/{id}", TransportTypeManagers.DeleteTransportType);
app.MapPost("/transport_type", TransportTypeManagers.CreateTransportType);
app.MapPut("/transport_type", TransportTypeManagers.UpdateTransportType);

//CRUD FOR TransportMode
app.MapGet("/transport_mode", TransportModeManagers.GetAllTransportMode);
app.MapGet("/transport_mode/{id}", TransportModeManagers.GetTransportModeById);
app.MapDelete("/transport_mode/{id}", TransportModeManagers.DeleteTransportMode);
app.MapPost("/transport_mode", TransportModeManagers.CreateTransportMode);
app.MapPut("/transport_mode", TransportModeManagers.UpdateTransportMode);

//CRUD FOR CoefficientType
app.MapGet("/coefficient_type", CoefficientTypeManagers.GetAllCoefficientType);
app.MapGet("/coefficient_type/{id}", CoefficientTypeManagers.GetCoefficientTypeById);
app.MapDelete("/coefficient_type/{id}", CoefficientTypeManagers.DeleteCoefficientType);
app.MapPost("/coefficient_type", CoefficientTypeManagers.CreateCoefficientType);
app.MapPut("/coefficient_type", CoefficientTypeManagers.UpdateCoefficientType);

//CRUD FOR COMPANY TRANSPORT
app.MapGet("/company_transport", CompanyTransportManagers.GetAllCompanyTransport);
app.MapGet("/company_transport/{id}", CompanyTransportManagers.GetCompanyTransportById);
app.MapDelete("/company_transport/{id}", CompanyTransportManagers.DeleteCompanyTransport);
app.MapPost("/company_transport", CompanyTransportManagers.CreateCompanyTransport);
app.MapPut("/company_transport", CompanyTransportManagers.UpdateCompanyTransport);

//CRUD FOR DeliveryAbility
app.MapGet("/delivery_ability", DeliveryAbilityManagers.GetAllDeliveryAbility);
app.MapGet("/delivery_ability/{id}", DeliveryAbilityManagers.GetDeliveryAbilityById);
app.MapDelete("/delivery_ability/{id}", DeliveryAbilityManagers.DeleteDeliveryAbility);
app.MapPost("/delivery_ability", DeliveryAbilityManagers.CreateDeliveryAbility);
app.MapPut("/delivery_ability", DeliveryAbilityManagers.UpdateDeliveryAbility);

//CRUD FOR USER TYPE 
app.MapGet("/user_type", UserTypeManagers.GetAllUserType);
app.MapGet("/user_type/{id}", UserTypeManagers.GetUserTypeById);
app.MapDelete("/user_type/{id}", UserTypeManagers.DeleteUserType);
app.MapPost("/user_type", UserTypeManagers.CreateUserType);
app.MapPut("/user_type", UserTypeManagers.UpdateUserType);

//CRUD FOR USER  
app.MapGet("/users", UserManagers.GetAllUser);
app.MapGet("/user/{id:int}", UserManagers.GetUserById);
app.MapDelete("/user/{id:int}", UserManagers.DeleteUser);
app.MapPost("/user", UserManagers.CreateUser);
app.MapPatch("/user/{id:int}", UserManagers.UpdateUser);

app.MapGet("/construction_types", ConstructionTypeManager.GetTypes);


app.MapGet("/subsidiaries",SubsidiaryManager.GetSubsidiaries);

app.Run();
