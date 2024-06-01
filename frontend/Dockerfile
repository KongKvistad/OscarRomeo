# Stage 1: Build the Next.js app
FROM node:18.17.0 AS builder

# Set the working directory
WORKDIR /app

# Copy package.json and package-lock.json to the working directory
COPY package.json package-lock.json ./

# Install dependencies
RUN npm install

# Copy the rest of the application files to the working directory
COPY . .

# Build the application
RUN npm run build

# Stage 2: Serve the Next.js app
FROM node:18.17.0 AS runner

# Set the working directory
WORKDIR /app

# Copy the build output from the builder stage
COPY --from=builder /app ./

# Expose the port
EXPOSE 3000

# Start the Next.js application
CMD ["npm", "run", "start"]
