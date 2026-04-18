import { useEffect } from "react";
import { useForm } from "react-hook-form";
import { useQuery, useMutation, useQueryClient } from "@tanstack/react-query";
import userService from "@/services/userService";

const JLPT_LEVELS = ["N5", "N4", "N3", "N2", "N1"] as const;

const SettingsPage = () => {
  const queryClient = useQueryClient();

  const { data, isLoading } = useQuery<UserSettings>({
    queryKey: ["settings"],
    queryFn: () => userService.getSettings(),
  });

  const {
    register,
    handleSubmit,
    reset,
    formState: { errors },
  } = useForm<UserSettings>();

  useEffect(() => {
    if (data) {
      reset(data);
    }
  }, [data, reset]);

  const mutation = useMutation<void, Error, UserSettings>({
    mutationFn: (dto) => userService.updateSettings(dto),
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: ["settings"] });
    },
  });

  const onSubmit = (values: UserSettings) => {
    mutation.reset();
    mutation.mutate(values);
  };

  if (isLoading) {
    return <div className="mx-auto my-24 max-w-md p-4">Loading...</div>;
  }

  return (
    <div className="mx-auto my-24 max-w-md p-4">
      <h1 className="text-2xl font-bold mb-4">Settings</h1>
      <form onSubmit={handleSubmit(onSubmit)}>
        <div className="flex flex-col gap-y-2 mb-4">
          <label htmlFor="dailyLessonLimit" className="text-xl">
            Daily Lesson Limit
          </label>
          <input
            type="number"
            id="dailyLessonLimit"
            className="w-80 p-2 text-xl"
            {...register("dailyLessonLimit", {
              valueAsNumber: true,
              min: { value: 1, message: "Minimum value is 1" },
              max: { value: 50, message: "Maximum value is 50" },
            })}
          />
          {errors.dailyLessonLimit && (
            <p className="text-red-600" role="alert">
              {errors.dailyLessonLimit.message}
            </p>
          )}
        </div>
        <div className="flex flex-col gap-y-2 mb-4">
          <label htmlFor="reviewBatchSize" className="text-xl">
            Review Batch Size
          </label>
          <input
            type="number"
            id="reviewBatchSize"
            className="w-80 p-2 text-xl"
            {...register("reviewBatchSize", {
              valueAsNumber: true,
              min: { value: 10, message: "Minimum value is 10" },
              max: { value: 200, message: "Maximum value is 200" },
            })}
          />
          {errors.reviewBatchSize && (
            <p className="text-red-600" role="alert">
              {errors.reviewBatchSize.message}
            </p>
          )}
        </div>
        <div className="flex flex-col gap-y-2 mb-4">
          <label htmlFor="jlptLevel" className="text-xl">
            JLPT Level
          </label>
          <select
            id="jlptLevel"
            className="w-80 p-2 text-xl"
            {...register("jlptLevel")}
          >
            {JLPT_LEVELS.map((level) => (
              <option key={level} value={level}>
                {level}
              </option>
            ))}
          </select>
        </div>
        {mutation.isSuccess && (
          <p className="mt-3 text-green-600" role="status">
            Settings saved successfully.
          </p>
        )}
        {mutation.isError && (
          <p className="mt-3 text-red-600" role="alert">
            {mutation.error.message}
          </p>
        )}
        <button
          type="submit"
          className="text-xl mt-8"
          disabled={mutation.isPending}
        >
          {mutation.isPending ? "Saving..." : "Save Settings"}
        </button>
      </form>
    </div>
  );
};

export default SettingsPage;
