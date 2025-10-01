# FHIR Patient Manager

A modern Blazor WebAssembly application for managing patient records using the FHIR (Fast Healthcare Interoperability Resources) REST API standard. Built with .NET 9 and connected to the public HAPI FHIR server for real-world healthcare data interoperability.

## Features

- **CRUD Operations**: Create, read, update, and delete patient records
- **Real-time Search**: Search patients by name or phone number
- **FHIR R4 Compliant**: Implements HL7 FHIR R4 specifications
- **Modern UI**: Clean, responsive interface built with Bootstrap 5
- **Live Data**: Connects to HAPI FHIR public server for testing and demonstration
- **Form Validation**: Client-side validation for patient data integrity
- **Progressive Web App**: Service worker support for offline capabilities

## Technology Stack

- **Framework**: Blazor WebAssembly (.NET 9)
- **FHIR Client**: Hl7.Fhir.R4 (v5.12.2)
- **UI Framework**: Bootstrap 5 with Bootstrap Icons
- **Architecture**: Service-oriented with clean separation of concerns

## Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0) or later
- A modern web browser (Chrome, Edge, Firefox, Safari)

## Getting Started

### Clone the Repository

```bash
git clone https://github.com/yourusername/FhirPatientManager.git
cd FhirPatientManager
```

### Run the Application

```bash
dotnet watch --project src/Client/FhirPatientManager.Client.csproj
```

The application will open automatically in your default browser at `http://localhost:5193`.

### Build for Production

```bash
dotnet publish src/Client/FhirPatientManager.Client.csproj -c Release
```

The compiled output will be in `src/Client/bin/Release/net9.0/publish/wwwroot/`.

## Project Structure

```
FhirPatientManager/
├── src/
│   └── Client/
│       ├── Models/
│       │   └── PatientViewModel.cs      # Patient data model with validation
│       ├── Services/
│       │   ├── IFhirService.cs          # Service contract
│       │   └── FhirService.cs           # FHIR API implementation
│       ├── Pages/
│       │   ├── Home.razor               # Landing page
│       │   ├── PatientList.razor        # Patient listing and search
│       │   └── PatientForm.razor        # Create/edit patient form
│       ├── Layout/
│       │   └── MainLayout.razor         # Application layout
│       └── wwwroot/                     # Static assets
└── FhirPatientManager.sln
```

## Architecture

### Service Layer

The application uses a clean service architecture with dependency injection:

- **IFhirService**: Interface defining patient operations
- **FhirService**: Implementation using HL7 FHIR .NET library
- **HttpClient**: Configured for Blazor WebAssembly compatibility

### Data Model

The `PatientViewModel` includes:
- Given Name and Family Name
- Gender (Male, Female, Other, Unknown)
- Date of Birth
- Phone Number
- Built-in data validation attributes

### FHIR Integration

Connects to the HAPI FHIR public test server:
- **Base URL**: `https://hapi.fhir.org/baseR4`
- **Resource Type**: Patient (FHIR R4)
- **Operations**: Search, Read, Create, Update, Delete

> [!NOTE]
> The HAPI FHIR public server is a test server with shared data. Patient records created during testing may be visible to other users and can be deleted at any time.

## Key Components

### Patient List

- Displays up to 20 patients from the FHIR server
- Real-time search by name or phone
- Card-based responsive layout
- Quick actions for edit and delete

### Patient Form

- Create new patients or edit existing ones
- Form validation with error messages
- Support for all patient demographics
- Cancel/save workflow

### FHIR Service

Implements the complete CRUD interface:

```csharp
Task<List<PatientViewModel>> GetAllPatientsAsync();
Task<PatientViewModel?> GetPatientByIdAsync(string id);
Task<List<PatientViewModel>> SearchPatientsAsync(string searchTerm);
Task<PatientViewModel> CreatePatientAsync(PatientViewModel patient);
Task<PatientViewModel> UpdatePatientAsync(PatientViewModel patient);
Task<bool> DeletePatientAsync(string id);
```

## Development

### Hot Reload

The application supports .NET hot reload for rapid development:

```bash
dotnet watch --project src/Client/FhirPatientManager.Client.csproj
```

Press `Ctrl+R` to restart the application.

### Adding Features

1. **New Models**: Add to `Models/` folder
2. **New Services**: Implement in `Services/` folder with interface
3. **New Pages**: Create `.razor` files in `Pages/` folder
4. **Routing**: Use `@page` directive for URL routing

## FHIR Resources

- [HL7 FHIR Specification](https://www.hl7.org/fhir/)
- [HAPI FHIR Server](https://hapi.fhir.org/)
- [HL7 FHIR .NET Library](https://github.com/FirelyTeam/firely-net-sdk)

## Browser Compatibility

- Chrome/Edge (latest)
- Firefox (latest)
- Safari (latest)
- Mobile browsers with WebAssembly support

## Troubleshooting

### CORS Issues

If you encounter CORS errors, ensure you're accessing the application via the correct localhost port. The HAPI FHIR server allows requests from any origin.

### Build Errors

Make sure you have .NET 9 SDK installed:

```bash
dotnet --version
```

Should return `9.0.x` or later.

### Service Worker

If the service worker isn't working correctly in development, clear your browser cache and reload the page.

## Performance

- Initial load: ~2-3 seconds (WebAssembly bootstrap)
- FHIR API calls: 200-800ms (depending on network)
- Client-side rendering: Near instant after initial load

## Security Considerations

> [!WARNING]
> This is a demonstration application connecting to a public test server. Do not use it for production healthcare data or any sensitive patient information.

For production use:
- Implement proper authentication (OAuth 2.0, OpenID Connect)
- Use a private FHIR server with access controls
- Enable HTTPS with valid certificates
- Implement audit logging for all patient data access
- Follow HIPAA compliance guidelines if handling US patient data

## Future Enhancements

- [ ] Authentication and authorization
- [ ] Pagination for large patient lists
- [ ] Advanced search filters (age, location, etc.)
- [ ] Bulk import/export functionality
- [ ] Patient observation and condition management
- [ ] Appointment scheduling
- [ ] Document attachment support

## Acknowledgments

- Built with [Blazor WebAssembly](https://dotnet.microsoft.com/apps/aspnet/web-apps/blazor)
- FHIR implementation using [Firely .NET SDK](https://github.com/FirelyTeam/firely-net-sdk)
- Test data provided by [HAPI FHIR](https://hapi.fhir.org/)
