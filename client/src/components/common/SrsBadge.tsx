type SrsBadgeProps = {
  stage: string;
};

const getStageBgColor = (stage: string): string => {
  if (stage.startsWith('Apprentice')) return 'bg-pink-600';
  if (stage.startsWith('Guru')) return 'bg-purple-600';
  if (stage === 'Master') return 'bg-blue-600';
  if (stage === 'Enlightened') return 'bg-cyan-600';
  if (stage === 'Burned') return 'bg-amber-600';
  return 'bg-gray-600';
};

const SrsBadge = ({ stage }: SrsBadgeProps) => {
  const bgColor = getStageBgColor(stage);
  return (
    <span className={`text-xs px-1.5 py-0.5 rounded text-white font-medium ${bgColor}`}>
      {stage}
    </span>
  );
};

export default SrsBadge;
