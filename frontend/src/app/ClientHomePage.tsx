// src/app/ClientHomePage.tsx
"use client";

import { useState, useEffect } from 'react';
import dynamic from 'next/dynamic';
import { Station } from './page'; // Import Station type from page.tsx

const MapComponent = dynamic(() => import('../components/Map'), {
  ssr: false,
});

interface ClientHomePageProps {
  stationData: Station[];
}

const ClientHomePage: React.FC<ClientHomePageProps> = ({ stationData }) => {
  const [stations, setStations] = useState<Station[]>(stationData);
  const [selectedStation, setSelectedStation] = useState<Station | null>(stationData[0]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const fetchStationData = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetch(`${process.env.NEXT_PUBLIC_API_URL}/Station/stationinfo`);
      if (!response.ok) {
        throw new Error('Failed to fetch station data');
      }
      const data = await response.json();
      setStations(data);
      setSelectedStation(data[0]);
    } catch (err) {
      console.error('Error fetching station data:', err);
      setError('Failed to fetch station data. Please try again later.');
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (stations.length === 0) {
      fetchStationData();
    } else {
      console.log('Station Data:', stations); // Log the data to verify its structure
    }
  }, []);

  return (
    <div className="flex flex-col md:flex-row h-screen">
      <div className="order-2 md:order-1 md:w-1/2 h-1/2 md:h-full overflow-y-auto p-4">
        <h1 className="text-xl font-bold mb-4 text-black">Station Information</h1>
        {loading ? (
          <p className="text-black">Loading...</p>
        ) : error ? (
          <p className="text-red-500">{error}</p>
        ) : (
          <ul className="space-y-2">
            {stations.map((station) => (
              <li
                key={station.station_id}
                className="p-4 bg-gray-100 rounded cursor-pointer hover:bg-gray-200 text-black"
                onClick={() => setSelectedStation(station)}
              >
                <p className="font-semibold text-black">{station.name}</p>
                <p className="text-black">Capacity: {station.capacity}</p>
                <p className="text-black">Bikes Available: {station.num_bikes_available}</p>
                <p className="text-black">Docks Available: {station.num_docks_available}</p>
              </li>
            ))}
          </ul>
        )}
      </div>
      <div className="order-1 md:order-2 md:w-1/2 h-1/2 md:h-full flex-grow">
        {selectedStation ? (
          <MapComponent
            lat={selectedStation.lat}
            lon={selectedStation.lon}
            num_bikes_available={selectedStation.num_bikes_available}
            num_docks_available={selectedStation.num_docks_available}
          />
        ) : (
          <div className="flex items-center justify-center h-full">
            <p className="text-lg text-black">Select a station to view its location on the map.</p>
          </div>
        )}
      </div>
    </div>
  );
};

export default ClientHomePage;
