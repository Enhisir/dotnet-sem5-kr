import React, { createContext, useContext, useState, useEffect, ReactNode } from 'react';
import axios from '../client/axiosInstance.ts'; // Клиент для API-запросов

interface Profile {
  username: string;
  rating: number;
}

interface ProfileContextType {
  profile: Profile | null;
  loading: boolean;
}

const ProfileContext = createContext<ProfileContextType | null>(null);

export const useProfile = () => {
  const context = useContext(ProfileContext);
  if (!context) {
    throw new Error("useProfile must be used within a ProfileProvider");
  }
  return [context.profile, context.loading] as const;
};

export const ProfileProvider = ({ children }: { children: ReactNode }) => {
  const [profile, setProfile] = useState<Profile | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchProfile = async () => {
      try {
        const response = await axios.get('/api/v1/users/rating');
        setProfile({ username: response.data.username, rating: response.data.rating });
      } catch {
        setProfile(null);
      } finally {
        setLoading(false);
      }
    };

    fetchProfile();
  }, []);

  return (
    <ProfileContext.Provider value={{ profile, loading }}>
      {children}
    </ProfileContext.Provider>
  );
};
