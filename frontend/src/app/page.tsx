import { useState } from 'react';
import dynamic from 'next/dynamic';
import ClientHomePage from './ClientHomePage';

interface Station {
  station_id: string;
  name: string;
  lat: number;
  lon: number;
  capacity: number;
}

interface StationInformation {
  data: {
    stations: Station[];
  };
}

async function fetchStationData(): Promise<StationInformation> {
  const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/station/stationinfo`);
  if (!response.ok) {
    throw new Error('Failed to fetch station data');
  }
  return response.json();
}

const HomePage = async () => {
  const stationData = await fetchStationData();

  return <ClientHomePage stationData={stationData} />;
};

export default HomePage;