using Core.DTOs.Cache;
using Data_Logger_1._3.ViewModels.Dialogs;
using Data_Logger_1._3.ViewModels.Dialogs.Edit;
using FileSys.DTOs;

namespace Data_Logger_1._3.Services
{
    internal class PostItCacheService
    {

        /// <summary>
        /// Prepares PostIts for a cached PostIt being edited
        /// </summary>
        /// <param name="navigationService">The navigation service dependency</param>
        /// <param name="editor"></param>
        /// <param name="postItCacheDTOs"></param>
        /// <param name="codingCacheDTO"></param>
        /// <returns></returns>
        public async Task<codeEditViewModel> PrepareCodingPostItViewModelEditor(NavigationService navigationService, codeEditViewModel editor, 
            IReadOnlyList<PostItCacheDTO> postItCacheDTOs, CodingCacheDTO codingCacheDTO)
        {
            // TODO: Fix docs and update after viewmodels are refactored.
            foreach (var postIt in postItCacheDTOs)
            {
                var project = postIt.Subject;

                var editPostIt = new EditPostItViewModel(navigationService, dataService, codingEditor, logCacheDTO.Project,
                    new(this, dataService, codingEditor, project,
                        postIt.Subject.Subject, postIt.Error, postIt.ERCaptureTime, postIt.Solution,
                        postIt.SOCaptureTime, postIt.Suggestion, postIt.Comment));
                await editPostIt.AutoStartAsync(codingCacheDTO.ApplicationName, codingCacheDTO.ProjectName);

                editor.PostIts.Add(editPostIt);
            }

            return editor;
        }
    }
}
