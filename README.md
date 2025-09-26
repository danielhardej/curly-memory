# Memory Game

A full-stack memory game application built with C# Web API backend and React TypeScript frontend.

## 🎮 Features

- Classic memory card matching game
- Multiple grid sizes (2x2, 4x4, 6x6)
- Move counter and timing
- Animated card flips
- Responsive design
- RESTful API backend
- Comprehensive test coverage

## 🏗️ Architecture

### Backend (C# Web API)
- **Framework**: ASP.NET Core 9.0
- **Testing**: xUnit
- **API Documentation**: OpenAPI/Swagger
- **CORS**: Enabled for frontend integration

### Frontend (React TypeScript)
- **Framework**: React 18 with TypeScript
- **Styling**: CSS with animations
- **Testing**: Jest + React Testing Library
- **Build Tool**: Create React App

## 🚀 Getting Started

### Prerequisites
- .NET 9.0 SDK
- Node.js 20.x
- npm

### Backend Setup
```bash
cd backend
dotnet restore
dotnet build
dotnet run --project MemoryGameApi
```

The API will be available at `https://localhost:7253`

### Frontend Setup
```bash
cd frontend/memory-game
npm install
npm start
```

The React app will be available at `http://localhost:3000`

## 🧪 Testing

### Backend Tests
```bash
cd backend
dotnet test
```

### Frontend Tests
```bash
cd frontend/memory-game
npm test
```

## 📖 API Documentation

### Endpoints

#### Create Game
```http
POST /api/game
Content-Type: application/json

{
  "gridSize": 4
}
```

#### Get Game
```http
GET /api/game/{gameId}
```

#### Flip Card
```http
POST /api/game/{gameId}/flip
Content-Type: application/json

{
  "cardId": 1
}
```

#### Get All Games
```http
GET /api/game
```

## 🔄 CI/CD

This project includes GitHub Actions workflows for:

- **Backend CI**: Builds, tests, and analyzes the C# Web API
- **Frontend CI**: Builds, tests, and lints the React application
- **Full Stack CI**: Runs integration tests across both components

Status checks are configured to run on all pull requests to ensure code quality.

## 🎯 Game Rules

1. Click cards to flip them over
2. Try to find matching pairs
3. Matched pairs stay revealed
4. Complete the game by finding all pairs
5. Try to minimize your number of moves!

## 🔧 Development

### Project Structure
```
├── backend/
│   ├── MemoryGameApi/          # Web API project
│   ├── MemoryGameApi.Tests/    # Unit tests
│   └── MemoryGame.sln          # Solution file
├── frontend/
│   └── memory-game/            # React TypeScript app
├── .github/
│   └── workflows/              # CI/CD pipelines
└── README.md
```

### Available Scripts

**Backend:**
- `dotnet build` - Build the solution
- `dotnet test` - Run tests
- `dotnet run` - Start the API server

**Frontend:**
- `npm start` - Start development server
- `npm test` - Run tests
- `npm run build` - Build for production
- `npm run lint` - Run ESLint

## 📝 License

This project is open source and available under the [MIT License](LICENSE).