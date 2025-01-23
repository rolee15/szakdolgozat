import api from "@/services/kanaService";
import { useEffect, useState } from "react";
import { useParams } from "react-router-dom";

const CharacterDetail = () => {
  const { type, character } = useParams();
  const [charData, setCharData] = useState<KanaCharacter | null>(null);
  const [examples, setExamples] = useState<string[]>([]);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    const fetchData = async () => {
      if (!type || !character) return;

      try {
        const [charDetail, exampleData] = await Promise.all([
          api.getCharacterDetail(type, character),
          api.getExamples(type, character)
        ]);

        setCharData(charDetail);
        setExamples(exampleData);
      } catch (err) {
        setError(err instanceof Error ? err.message : 'Failed to load character data');
      }
    };

    fetchData();
  }, [type, character]);

  if (error) {
    return (
      <div className="p-4 text-red-500">
        Error: {error}
      </div>
    );
  }

  if (!charData) return <div className="p-4">Loading...</div>;

  return (
    <div className="max-w-2xl mx-auto p-4">
      <div className="bg-white rounded-lg shadow-lg p-6">
        <div className="text-center mb-8">
          <div className="text-8xl font-bold mb-4">{charData.character}</div>
          <div className="text-2xl text-gray-600">{charData.romanization}</div>
          <div className="mt-4 bg-blue-100 rounded-full px-4 py-2 inline-block">
            Proficiency: {charData.proficiency}%
          </div>
        </div>

        <div className="space-y-6">
          <section>
            <h2 className="text-xl font-bold mb-3">Practice Writing</h2>
            <div className="border-2 border-gray-300 rounded-lg p-4 h-40 flex items-center justify-center">
              <span className="text-gray-400">Writing practice area coming soon</span>
            </div>
          </section>

          <section>
            <h2 className="text-xl font-bold mb-3">Example Words</h2>
            <div className="grid grid-cols-2 gap-4">
              {examples.map((example, index) => (
                <div key={index} className="p-3 bg-gray-50 rounded-lg">
                  {example}
                </div>
              ))}
            </div>
          </section>
        </div>
      </div>
    </div>
  );
};

export default CharacterDetail;
