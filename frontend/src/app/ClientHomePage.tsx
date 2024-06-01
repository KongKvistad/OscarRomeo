// app/ClientHomePage.tsx
"use client";

import { useState } from 'react';
import dynamic from 'next/dynamic';

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

const MapComponent = dynamic(() => import('../components/Map'), {
  ssr: false,
});

interface ClientHomePageProps {
  stationData: StationInformation;
}

const ClientHomePage: React.FC<ClientHomePageProps> = ({ stationData }) => {
  const [selectedStation, setSelectedStation] = useState<Station | null>(null);

  return (
    <div>
      <h1>Station Information</h1>
      <ul>
        {stationData.data.stations.map((station) => (
          <li key={station.station_id} onClick={() => setSelectedStation(station)}>
            <p>{station.name}</p>
            <p>Capacity: {station.capacity}</p>
          </li>
        ))}
      </ul>
      {selectedStation && (
        <div>
          <h2>Selected Station: {selectedStation.name}</h2>
          <MapComponent lat={selectedStation.lat} lon={selectedStation.lon} />
        </div>
      )}
    </div>
  );
};

export default ClientHomePage;
