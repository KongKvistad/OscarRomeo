# Stage 1: Base Node Image
FROM node:20.10.0

WORKDIR /app

ENV NEXT_PUBLIC_API_URL=http://localhost:5000

# Copy package.json and package-lock.json
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application code
COPY . .

# Build the application
RUN npm run build

# Expose the port the app runs on
EXPOSE 3000

# Start the application
CMD ["npm", "run", "start"]