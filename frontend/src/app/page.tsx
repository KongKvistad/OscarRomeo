// src/app/page.tsx
import { Suspense } from 'react';
import ClientHomePage from './ClientHomePage';

export interface Station {
  station_id: string;
  name: string;
  lat: number;
  lon: number;
  capacity: number;
  num_bikes_available: number;
  num_docks_available: number;
}

async function fetchStationData(): Promise<Station[]> {
  const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Station/stationinfo`);
  if (!response.ok) {
    throw new Error('Failed to fetch station data');
  }
  const data = await response.json();
  console.log('Fetched Station Data:', data);  // Log the stations array to verify its structure
  return data
}

const HomePage = async () => {
  const stationData = await fetchStationData();

  return (
    <div>
      <h1>Station Information</h1>
      <Suspense fallback={<div>Loading...</div>}>
        <ClientHomePage stationData={stationData} />
      </Suspense>
    </div>
  );
};

export default HomePage;
