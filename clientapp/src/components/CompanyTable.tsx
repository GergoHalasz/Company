import React, { useEffect, useState } from "react";
import axios from "axios";

interface Company {
  id: number;
  name: string;
  exchange: string;
  ticker: string;
  isin: string;
  websiteUrl?: string;
}

const API_BASE_URL = "https://localhost:7185";

const CompanyTable: React.FC = () => {
  const [companies, setCompanies] = useState<Company[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [token, setToken] = useState<string | null>(null);

  useEffect(() => {
    const loginAndFetchCompanies = async () => {
      try {
        const loginResponse = await axios.post(
          `${API_BASE_URL}/api/auth/login`,
          {
            username: "testUser",
            password: "testPassword",
          }
        );

        const jwtToken = loginResponse.data.token;
        setToken(jwtToken);

        const response = await axios.get(`${API_BASE_URL}/api/companies`, {
          headers: {
            Authorization: `Bearer ${jwtToken}`,
          },
        });

        setCompanies(response.data);
      } catch (error) {
        console.error("Error:", error);
        setError("Failed to load data.");
      } finally {
        setLoading(false);
      }
    };

    loginAndFetchCompanies();
  }, []);

  if (loading) return <p>Loading...</p>;
  if (error) return <p style={{ color: "red" }}>{error}</p>;

  return (
    <div>
      <h2>Companies List</h2>
      <table
        border={1}
        cellPadding={10}
        style={{ width: "100%", borderCollapse: "collapse" }}
      >
        <thead>
          <tr>
            <th>ID</th>
            <th>Name</th>
            <th>Exchange</th>
            <th>Ticker</th>
            <th>ISIN</th>
            <th>Website</th>
          </tr>
        </thead>
        <tbody>
          {companies.map((company) => (
            <tr key={company.id}>
              <td>{company.id}</td>
              <td>{company.name}</td>
              <td>{company.exchange}</td>
              <td>{company.ticker}</td>
              <td>{company.isin}</td>
              <td>
                {company.websiteUrl ? (
                  <a
                    href={company.websiteUrl}
                    target="_blank"
                    rel="noopener noreferrer"
                  >
                    {company.websiteUrl}
                  </a>
                ) : (
                  "N/A"
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default CompanyTable;
