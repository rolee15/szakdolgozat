type User = {
    id: string;
    email: string;
    username: string;
};

interface UserSettings {
    dailyLessonLimit: number;
    reviewBatchSize: number;
    jlptLevel: string;
}