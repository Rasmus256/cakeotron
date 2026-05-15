# CakeOTron UI

A whimsical Angular front-end for the CakeOTron backend services.

## Local development

This UI always connects to the external CakeOTron API at `https://cake.hosrasmus.hopto.org/api`.

1. From the repository root:
   ```bash
   cd cakeotron-ui
   npm install
   npm start
   ```
2. Open `http://localhost:4200` in your browser. The app will call the external host directly.

## Configuration

- The backend URL is configured in `src/environments/environment.ts` and `src/environments/environment.prod.ts`.
- If your `mainservice` runs on a different host or port, update `apiBaseUrl`.

## Docker deployment

Build the UI image from the repository root:

```bash
cd cakeotron-ui
docker build -t cakeotron-ui .
```

Run the UI container with the host network so it can reach the local `mainservice` at `localhost:5000`:

```bash
docker run --network host -p 8080:80 cakeotron-ui
```

Then open `http://localhost:8080`.

If you prefer to run the backend locally, update `src/environments/environment.ts` to point to your local `mainservice` URL.

> Note: The app is configured to call `https://cake.hosrasmus.hopto.org` by default; change the values in the environment files if you need a different target.
